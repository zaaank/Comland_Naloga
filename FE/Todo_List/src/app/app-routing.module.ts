import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TodoListComponent } from './todo-list/todo-list.component';
import { TodoListFormComponent } from './todo-list-form/todo-list-form.component';

const routes: Routes = [
  {
    path: 'todo-list',
    component: TodoListComponent,
  },
  {
    path: 'todo-list-form',
    component: TodoListFormComponent
  },
  {
    path: '**',
    redirectTo: 'todo-list',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
