import { httpClient } from './httpClient'
import type { CreateDishDto, DishDto } from '../types/dish'

export async function getDishesByMenuId(menuId: number): Promise<DishDto[]> {
  const response = await httpClient.get<DishDto[]>(`/menus/${menuId}/dishes`)

  return response.data
}

export async function createDishForMenu(
  menuId: number,
  dish: CreateDishDto,
): Promise<DishDto> {
  const response = await httpClient.post<DishDto>(
    `/menus/${menuId}/dishes`,
    dish,
  )

  return response.data
}
export async function deleteDish(id: number): Promise<void> {
  await httpClient.delete(`/dishes/${id}`)
}