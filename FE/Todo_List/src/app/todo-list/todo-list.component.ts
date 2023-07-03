import { Component, OnInit } from '@angular/core';
import { TaskModel, TaskStatusEnum } from './todo-list.model';
import { Router } from '@angular/router';
import { ManageTasksService } from '../services/manage-tasks.service';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements OnInit {

  tasks: TaskModel[] = [];
  taskStatusEnum = TaskStatusEnum;
  searchValue: string = '';
  constructor(private router: Router, private manageTasksService: ManageTasksService) { }

  ngOnInit() {
    this.manageTasksService.allTasks.subscribe((tasks: TaskModel[]) => {
      this.tasks = tasks
      this.searchValue = '';
    })
  }

  ngAfterViewInit() {
    this.setTableColumnsWidthDynamically();
  }

  setTableColumnsWidthDynamically() {
    //This will dinamically set the width of # Column, based on the biggest id width
    const column = document.querySelector('.smallest-width');
    const maxWidth = Math.max(...Array.from(document.querySelectorAll('.smallest-width')).map(el => el.getBoundingClientRect().width));
    column?.setAttribute('style', `width: ${maxWidth}px`);

    //This will dinamically set the width of column Title (40% of remaining width) and Desc (60% of remaining width)
    const titleColumn = document.querySelector('.title-column');
    const descColumn = document.querySelector('.desc-column');
    if (titleColumn && descColumn) {
      const totalWidth = titleColumn.getBoundingClientRect().width + descColumn.getBoundingClientRect().width;
      const titleColumnWidth = totalWidth * 0.4;
      const descColumnWidth = totalWidth * 0.6;
      titleColumn.setAttribute('style', `width: ${titleColumnWidth}px`);
      descColumn.setAttribute('style', `width: ${descColumnWidth}px`);
    }
  }

  addTask() {
    this.router.navigate(['/todo-list-form']);
  }

  markAsDone(task: TaskModel) {
    this.manageTasksService.markAsDone(task.id);
  }

  editTask(task: TaskModel) {
    this.router.navigate(['/todo-list-form'], { state: task });
  }

  deleteTask(task: TaskModel) {
    this.manageTasksService.deleteTask(task.id);
  }

  filterTasks() {
    this.tasks = this.manageTasksService.searchTasks(this.searchValue);
  }

  orderByTasks(fieldName: string) {
    //here we also send tasks in case user has filtered them before making order by
    this.tasks = this.manageTasksService.orderByTasks(this.tasks, fieldName);
  }
}
