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
                if (data.categories.Count > 1
                    && category.data.Label.Contains(DataBaseStandards.R_NOCATEGORY)
                    && (
                        category.projects.Count < 1 ||
                        (category.projects.Count == 1 && category.projects[0].tasks.Count <= 0 && category.projects[0].appointments.Count <= 0)
                    )) continue;
                categories.Add(new Classes.DataView.Category
                {
                    Id = category.Id,
                    Label = category.data.Label.Contains(DataBaseStandards.R_NOCATEGORY) ? "Uncategorized" : category.data.Label
                });
            }
            return categories;
        }

        public ObservableCollection<DataView.Project> LoadProjects(DataStructure data, DataView.Category pSelectedCategory)
        {
            ObservableCollection<DataView.Project> projects = [];
            foreach (Classes.DataStructure.Category category in data.categories)
            {
                //System.Diagnostics.Debug.WriteLine($"Selected Id: {pSelectedCategory.Id}");
                if (category.Id != pSelectedCategory.Id) continue;
                foreach (Classes.DataStructure.Category.Project project in category.projects)
                {
                    projects.Add(new Classes.DataView.Project
                    {
                        Id = project.Id,
                        Label = project.data.Label.Contains(DataBaseStandards.R_NOPROJECT) ? "Unassigned" : project.data.Label
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
                        Tasks.Add(new Classes.DataView.Task(data, task.priority.Id, (task.difficulty != null ? task.difficulty.Value.Id : null))
                        {
                            Id = task.Id,
                            Label = task.data.Label,
                            IsFinished = task.data.Finished.HasValue
                        });
                    }
                    break;
                }
                break;
            }
            return Tasks;
        }

        public ObservableCollection<DataView.Difficulty> LoadDifficulties(DataStructure data)
        {
            ObservableCollection<DataView.Difficulty> difficulties = [];
            foreach (Classes.DataStructure.InternalDifficulty difficulty in data.difficulties)
            {
                difficulties.Add(new Classes.DataView.Difficulty
                {
                    Id = difficulty.Id,
                    Label = difficulty.Label,
                    Description = difficulty.Description,
                    Recommendation = difficulty.Recommendation,
                    Color = difficulty.Color
                });
            }
            difficulties.Add(new Classes.DataView.Difficulty {
                Id = 0,
                Label = "<none>",
                Description = "You have not selected a difficulty for this task.",
                Recommendation = "There is no recommendation for tasks that have no difficulty set."
            });
            return difficulties;
        }

        public ObservableCollection<DataView.Priority> LoadPriorites(DataStructure data)
        {
            ObservableCollection<DataView.Priority> priorities = [];
            foreach (Classes.DataStructure.InternalPriority priority in data.priorities)
            {
                priorities.Add(new Classes.DataView.Priority
                {
                    Id = priority.Id,
                    Label = priority.Label,
                    Ordering = priority.Ordering,
                    Color = priority.Color
                });
            }
            return priorities;
        }
    }
}
