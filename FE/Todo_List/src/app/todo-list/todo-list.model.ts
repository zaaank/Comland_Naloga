export interface TaskModel {
  id: number,
  title: string,
  description: string,
  status: TaskStatusEnum
}

export enum TaskStatusEnum {
  'ToDo' = 1,
  'Done' = 2
}
