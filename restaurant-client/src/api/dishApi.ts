import { httpClient } from './httpClient'
import type { DishDto } from '../types/dish'

export async function getDishesByMenuId(menuId: number): Promise<DishDto[]> {
  const response = await httpClient.get<DishDto[]>(`/menus/${menuId}/dishes`)

  return response.data
}