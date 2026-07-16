using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static Tasker.Classes.DataBase;

namespace Tasker.Classes
{
    //Class to load, save and change data from DataStructure to View.
    public class ViewControl
    {
        public ObservableCollection<DataView.Category> LoadCategories(DataStructure data)
        {
            ObservableCollection<DataView.Category> categories = [];
            foreach (Classes.DataStructure.Category category in data.categories)
            {
                categories.Add(new Classes.DataView.Category
                {
                    Id = category.Id,
                    Label = category.data.Label.Contains("RESERVED_NOCATEGORY") ? "Allgemein" : category.data.Label
                });
            }
            return categories;
        }

        public ObservableCollection<DataView.Project> LoadProjects(DataStructure data, DataView.Category pSelectedCategory)
        {
            ObservableCollection<DataView.Project> projects = [];
            foreach (Classes.DataStructure.Category category in data.categories)
            {
                if (category.Id != pSelectedCategory.Id) continue;
                foreach (Classes.DataStructure.Category.Project project in category.projects)
                {
                    projects.Add(new Classes.DataView.Project
                    {
                        Id = project.Id,
                        Label = project.data.Label.Contains("RESERVED_NOPROJECT") ? "Allgemein" : project.data.Label
                    });
                }
                break;
            }
            return projects;
        }

        public ObservableCollection<DataView.Task> LoadTasks(DataStructure data, DataView.Category pSelectedCategory, DataView.Project pSelectedProject)
        {
            ObservableCollection<DataView.Task> Tasks = [];
            List<DataStructure.InternalPriority> Priorites = [];

            foreach (Classes.DataStructure.Category category in data.categories)
            {
                if (category.Id != pSelectedCategory.Id) continue;
                foreach (Classes.DataStructure.Category.Project project in category.projects)
                {
                    if (project.Id != pSelectedProject.Id) continue;
                    foreach (Classes.DataStructure.Category.Project.Task task in project.tasks)
                    {
                        Tasks.Add(new Classes.DataView.Task(data, task.priority.Id)
                        {
                            Id = task.Id,
                            Label = task.data.Label
                        });
                    }
                    break;
                }
                break;
            }
            return Tasks;
        }

        public ObservableCollection<DataView.Priority> LoadPriorites(DataStructure data)
        {
            ObservableCollection<DataView.Priority> priorities = [];
            foreach (Classes.DataStructure.InternalPriority priority in data.priorities)
            {
                priorities.Add(new Classes.DataView.Priority
                {
                    Id = priority.Id,
                    Label = priority.Label
                });
            }
            return priorities;
        }
    }
}
