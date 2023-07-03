import { Injectable } from '@angular/core';
import { TaskModel, TaskStatusEnum } from '../todo-list/todo-list.model';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ManageTasksService {

  allTasks: BehaviorSubject<TaskModel[]> = new BehaviorSubject<TaskModel[]>([]);

  constructor() {

    //first fetch tasks from localstorage
    const tasks = localStorage.getItem('tasks');
    if (tasks != null) {
      this.allTasks.next(JSON.parse(tasks) as TaskModel[])
    }

    //now subscribe to changes on tasks
    this.allTasks.subscribe((tasks: TaskModel[]) => {
      this.renewLocalstorageTasks(tasks);
    })
  }

  addNewTask(title: string, description: string) {
    // Determine the highest ID
    const taskIds = this.allTasks.getValue().map(task => task.id);
    const highestId = taskIds.length > 0 ? Math.max(...taskIds) : 0;

    // Generate a new unique ID
    const newId = highestId + 1;

    // Create a new task with the new ID
    const newTask: TaskModel = {
      id: newId,
      title: title,
      description: description,
      status: TaskStatusEnum.ToDo
    };


    const currentTasks = this.allTasks.getValue();
    currentTasks.push(newTask);
    this.allTasks.next(currentTasks);
  }

  updateExistingTask(task: TaskModel) {
    const tasks = this.allTasks.getValue();
    tasks.forEach(innerTask => {
      if (innerTask.id === task.id) {
        innerTask.description = task.description;
        innerTask.title = task.title;
        return; // Exit the loop once the ID is found
      }
    });
    this.allTasks.next(tasks);
  }

  deleteTask(id: number) {
    const filteredTasks = this.allTasks.getValue().filter(task => task.id !== id);
    this.allTasks.next(filteredTasks);
  }

  searchTasks(searchTerm: string): TaskModel[] {
    const tasks = this.allTasks.getValue();
    const matchingResults = tasks.filter(result =>
      result.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
      result.description.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return matchingResults;
  }

  orderByTasks(currentVisibleTasks: TaskModel[], orderByField: string): TaskModel[] {
    const sortOrder = "asc";

    // Custom sorting function based on selected field(s) and order
    const compareFunction = (a: any, b: any) => {
      if (a[orderByField] < b[orderByField]) {
        return sortOrder === 'asc' ? -1 : 1;
      }
      if (a[orderByField] > b[orderByField]) {
        return sortOrder === 'asc' ? 1 : -1;
      }
      return 0;
    };

    // Sort the array based on selected field(s) and order
    return currentVisibleTasks.sort(compareFunction);
  }

  markAsDone(id: number) {
    const tasks = this.allTasks.getValue();
    tasks.forEach(task => {
      if (task.id === id) {
        task.status = TaskStatusEnum.Done; // Replace with the status Done
        return; // Exit the loop once the ID is found
      }
    });
    this.allTasks.next(tasks);
  }

  renewLocalstorageTasks(tasks: TaskModel[]) {
    const stringifyTasks = JSON.stringify(tasks);
    localStorage.setItem('tasks', stringifyTasks);
  }

}
