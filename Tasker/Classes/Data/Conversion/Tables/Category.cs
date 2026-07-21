using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tasker.Classes.Data.Retrieval;
using Tasker.Classes.Templates;
using static Tasker.Classes.Data.Conversion.DataStructure;

namespace Tasker.Classes.Data.Conversion.Tables
{
    public class Category : ObservableObjectTemplate
    {
        private InternalPriority _selectedPriority;
        private ObservableCollection<Project> _projects;
        public InternalPriority SelectedPriority { get => _selectedPriority; set => Update(ref _selectedPriority, value); }

        public ObservableCollection<Project> Projects { get => _projects; set => _projects = value; }

        public Category(int pId, InternalData pData, InternalPriority pSelectedPriority) : base(pId, pData)
        {
            _selectedPriority = pSelectedPriority;
            _projects = [];
        }
        public void Load(List<DataBase.Project.ProjectData>? pProject = null, bool pClear = true)
        {
            if (pClear) _projects = [];
            if (pProject != null) LoadProjects(pProject);
        }

        public void CreateProject(object? pLabel)
        {
            if (pLabel == null || pLabel is not string newLabel || newLabel == "") return;
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
        }

        public void DeleteProject(object? pId)
        {
            if (pId == null || pId is not int projectId || projectId <= 0) return;
            try
            {
                Project project = _projects.Where(x => x.Id == projectId).ToArray()[0];
                if (_projects.Where(x => x.Data.Id == project.Data.Id).Count() == 1) DeleteData(project.Data.Id);
                new DataBase.Project().Delete(projectId);
                _projects.Remove(project);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Class -Category-; Critical Error while trying to delete project.\nMessage:{e}");
            }
        }

        private void LoadProjects(List<DataBase.Project.ProjectData> pProjects)
        {
            foreach (DataBase.Project.ProjectData project in pProjects) _projects.Add(GetProject(project));
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
