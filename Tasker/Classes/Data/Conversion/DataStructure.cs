using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using Tasker.Classes.Data.Conversion.Tables;
using Tasker.Classes.Data.Retrieval;
using static Tasker.Classes.Data.Conversion.DataBaseDataToTables;
using static Tasker.Classes.Data.Retrieval.DataBase;


namespace Tasker.Classes.Data.Conversion
{
    //Handles Visuals of the program
    public static class DataStructure
    {
        private static ObservableCollection<Tables.Category> _categories = [];

        private static ObservableCollection<InternalPriority> _priorities = [];
        private static ObservableCollection<InternalDifficulty> _difficulties = [];


        public static ObservableCollection<Tables.Category> Categories
        {
            get => _categories;
            set => _categories = value;
        }
        public static ObservableCollection<InternalPriority> Priorities
        {
            get => _priorities;
            set => _priorities = value;
        }
        public static ObservableCollection<InternalDifficulty> Difficulties
        {
            get => _difficulties;
            set => _difficulties = value;
        }


        public static void Reload()
        {
            _categories = [];
            _priorities = [];
            _difficulties = [];
            foreach (DataBase.Priority.PriorityData priority in new DataBase.Priority().Get())
            {
                _priorities.Add(GetPriority(priority));
            }
            foreach (DataBase.Difficulty.DifficultyData difficulty in new DataBase.Difficulty().Get())
            {
                _difficulties.Add(GetDifficulty(difficulty));
            }
            List<DataBase.Project.ProjectData> dbProjects = new DataBase.Project().Get();
            List<DataBase.Task.TaskData> dbTasks = new DataBase.Task().Get();
            List<DataBase.Appointment.AppointmentData> dbAppointments = new DataBase.Appointment().Get();
            foreach (DataBase.Category.CategoryData category in new DataBase.Category().Get())
            {
                Tables.Category newCategory = GetCategory(category);

                //System.Diagnostics.Debug.WriteLine($"Class -DataStructure; Init Category {newCategory.Id}");
                //System.Diagnostics.Debug.WriteLine($"Class -DataStructure; Size of dbProjects: {dbProjects.Count}");

                List<DataBase.Project.ProjectData> projectsInCategory = dbProjects.Where(project => project.CategoryId == newCategory.Id).ToList();
                dbProjects = dbProjects.Except(projectsInCategory).ToList();

                //System.Diagnostics.Debug.WriteLine($"Class -DataStructure; Size of Projects in Category: {projectsInCategory.Count}");

                newCategory.Load(projectsInCategory);
                foreach (Tables.Project project in newCategory.Projects)
                {
                    //System.Diagnostics.Debug.WriteLine($"Class -DataStructure; Init Project {project.Id}");
                    //System.Diagnostics.Debug.WriteLine($"Class -DataStructure; Size of dbTasks: {dbTasks.Count}");

                    List<DataBase.Task.TaskData> tasksInProject = dbTasks.Where(task => task.ProjectId == project.Id).ToList();
                    dbTasks = dbTasks.Except(tasksInProject).ToList();

                    //System.Diagnostics.Debug.WriteLine($"Class -DataStructure; Size of Tasks in Project: {tasksInProject.Count}");

                    List<DataBase.Appointment.AppointmentData> appointmentsInProject = dbAppointments.Where(appointment => appointment.ProjectId == project.Id).ToList();
                    dbAppointments = dbAppointments.Except(appointmentsInProject).ToList();

                    project.Load(tasksInProject, appointmentsInProject);
                }
                _categories.Add(newCategory);
            }
        }

        public static int CreateOrLoadData(string pLabel)
        {
            DataBase.Data dbData = new();
            if (dbData.Get().Where(x => x.Label == pLabel).Any()) return dbData.Get().Where(x => x.Label == pLabel).ToArray()[0].Id;
            int newDataId = dbData.GenerateId();
            dbData.Add(new DataBase.Data.DataOfData
            {
                Id = newDataId,
                Label = pLabel
            });
            return newDataId;
        }

        public static void DeleteData(int pId)
        {
            try { new DataBase.Data().Delete(pId); } catch (Exception e) { System.Diagnostics.Debug.WriteLine($"DATABASE ERROR WHILE TRYING TO DELETE DATA:\n{e}"); }
        }

        public static int CreateCategory(object? pLabel)
        {
            System.Diagnostics.Debug.WriteLine($"Class -DataStructure-; Trying to Create Category");
            if (pLabel == null || pLabel is not string newLabel || newLabel == "") return 1;
            int newDataId = CreateOrLoadData(newLabel);

            DataBase.Category dbCategory = new();

            int newCategoryId = dbCategory.GenerateId();

            dbCategory.Add(new DataBase.Category.CategoryData
            {
                Id = newCategoryId,
                DataId = newDataId,
                PriorityId = GetDefaultPriority().Id
            });

            Tables.Category newCategory = GetCategory(new DataBase.Category.CategoryData
            {
                Id = newCategoryId,
                DataId = newDataId,
                PriorityId = GetDefaultPriority().Id
            });

            newCategory.CreateProject(DataBaseStandards.R_NOPROJECT);
            _categories.Add(newCategory);
            System.Diagnostics.Debug.WriteLine($"Class -DataStructure-; Category Successfully created.");
            return newCategoryId;
        }

        public static void DeleteCategory(object? pId)
        {
            if (pId == null || pId is not int categoryId || categoryId <= 0) return;
            try
            {
                Tables.Category category = _categories.Where(x => x.Id == categoryId).ToArray()[0];
                if (_categories.Where(x => x.Data.Id == category.Data.Id).Count() == 1) DeleteData(category.Data.Id);
                new DataBase.Category().Delete(categoryId);
                _categories.Remove(category);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Class -DataStructure-; Critical Error while trying to delete category.\nMessage:{e}");
            }
        }

        private static DataBase.Priority.PriorityData GetDefaultPriority()
        {
            List<DataBase.Priority.PriorityData> dbPriority = new DataBase.Priority().Get();
            return dbPriority.Where(x => x.Label == "normal").ToArray().Length != 1
                ? dbPriority[0]
                : dbPriority.Where(x => x.Label == "normal").ToArray()[0];
        }
    }
}
