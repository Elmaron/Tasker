using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tasker.Classes.Data.Retrieval;
using Tasker.Classes.Templates;
using static Tasker.Classes.Data.Conversion.DataStructure;
using static Tasker.Classes.Data.Conversion.DataBaseDataToTables;

namespace Tasker.Classes.Data.Conversion.Tables
{
    public class Category : ObservableObjectTemplate
    {
        private InternalPriority _selectedPriority;
        private ObservableCollection<Project> _projects;
        private Project? _hiddenReserved;
        public InternalPriority SelectedPriority { get => _selectedPriority; set => Update(ref _selectedPriority, value); }

        public ObservableCollection<Project> Projects { get => _projects; set => _projects = value; }

        public bool ContainsObjects
        {
            get
            {
                foreach (Project project in _projects) {
                    if(project.ContainsObjects) return true;
                }
                return false;
            }
        }

        public Category(int pId, InternalData pData, InternalPriority pSelectedPriority) : base(pId, pData)
        {
            _selectedPriority = pSelectedPriority;
            _projects = [];
            System.Diagnostics.Debug.WriteLine($"Class -Category-; Found: {Data.Label}");
        }
        public void Load(List<DataBase.Project.ProjectData>? pProject = null, bool pClear = true)
        {
            if (pClear) _projects = [];
            if (pProject != null && pProject.Count >= 1) LoadProjects(pProject);
        }

        public int CreateProject(object? pLabel)
        {
            if (pLabel == null || pLabel is not string newLabel || newLabel == "") return 1;
            int newDataId = CreateOrLoadData(newLabel);

            DataBase.Project dbProject = new();

            int newProjectId = dbProject.GenerateId();

            dbProject.Add(new DataBase.Project.ProjectData
            {
                Id = newProjectId,
                DataId = newDataId,
                CategoryId = Id,
                PriorityId = _selectedPriority.Id
            });

            _projects.Add(GetProject(new DataBase.Project.ProjectData
            {
                Id = newProjectId,
                DataId = newDataId,
                CategoryId = Id,
                PriorityId = _selectedPriority.Id
            }));

            if (_hiddenReserved != null) return newProjectId;

            _hiddenReserved = _projects.FirstOrDefault(x => x.Data.IsReserved && !x.ContainsObjects);
            if (_hiddenReserved != null) _projects.Remove(_hiddenReserved);
            return newProjectId;
        }

        public void DeleteProject(object? pId)
        {
            if (pId == null || pId is not int projectId || projectId <= 0) return;
            try
            {
                Project project = _projects.Where(x => x.Id == projectId).ToArray()[0];
                foreach (Task task in project.Tasks) project.DeleteTask(task.Id);
                foreach (Appointment appointment in project.Appointments) project.DeleteAppointment(appointment.Id);
                new DataBase.Project().Delete(projectId);
                if (new DataBase.Data().Get().Where(x => x.Id == project.Data.Id).Count() == 1) DeleteData(project.Data.Id);
                _projects.Remove(project);
                if (_projects.Count != 0 || _hiddenReserved == null) return;
                _projects.Add(_hiddenReserved);
                _hiddenReserved = null;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Class -Category-; Critical Error while trying to delete project.\nMessage:{e}");
            }
        }

        public static void CreateCategory(object? label) { System.Diagnostics.Debug.WriteLine($"Class -Category-; Trying to Create new category"); DataStructure.CreateCategory(label); }
        public void DeleteCategory() { DataStructure.DeleteCategory(Id); }

        private void LoadProjects(List<DataBase.Project.ProjectData> pProjects)
        {
            if (pProjects.Count != 1)
            foreach (DataBase.Project.ProjectData projectData in pProjects)
            {
                Project project = GetProject(projectData);
                if (_hiddenReserved != null || !project.Data.IsReserved || project.ContainsObjects) { _projects.Add(project); continue; }
                _hiddenReserved = project;
            }
            else _projects.Add(GetProject(pProjects.First()));
        }

        private void Update<T>(ref T field, T value)
        {
            field = value;
            new DataBase.Project().Update(new DataBase.Project.ProjectData
            {
                Id = Id,
                PriorityId = _selectedPriority.Id
            });
        }
    }
}
