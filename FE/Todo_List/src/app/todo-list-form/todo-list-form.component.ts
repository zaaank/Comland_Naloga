import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ManageTasksService } from '../services/manage-tasks.service';
import { NavigationEnd, Router } from '@angular/router';
import { TaskModel } from '../todo-list/todo-list.model';

@Component({
  selector: 'app-todo-list-form',
  templateUrl: './todo-list-form.component.html',
  styleUrls: ['./todo-list-form.component.css']
})
export class TodoListFormComponent {
  todoForm!: FormGroup;
  editedTask: TaskModel | null = null;
  constructor(private formBuilder: FormBuilder, private manageTasksService: ManageTasksService, private router: Router) {
    this.initFormGroup();
    this.checkExistingState();
  }

  initFormGroup() {
    this.todoForm = this.formBuilder.group({
      title: ['', Validators.required],
      description: ['', Validators.required]
    });
  }

  checkExistingState() {
    const task: TaskModel | null = this.router.getCurrentNavigation()?.extras.state as TaskModel | null
    console.log("call router");
    if (task != null) {
      this.editedTask = task
      const newValues = {
        title: task.title,
        description: task.description
      };
      this.todoForm.setValue(newValues);
    }
  }

  onSubmit() {
    const title = this.todoForm.value.title;
    const description = this.todoForm.value.description;
    this.todoForm.reset();
    //update existing task
    if (this.editedTask) {
      const updatedTask = {...this.editedTask, description, title};
      this.manageTasksService.updateExistingTask(updatedTask);
    }
    //create new task
    else {
      this.manageTasksService.addNewTask(title, description);
    }
    this.cancel();
  }

  cancel() {
    this.router.navigate(['/todo-list']);
  }

}
