import { httpClient } from './httpClient'
import type { CreateMenuDto, MenuDto, UpdateMenuDto } from '../types/menu'

export async function getMenuByRestaurantId(
  restaurantId: number,
): Promise<MenuDto | null> {
  try {
    const response = await httpClient.get<MenuDto>(
      `/restaurants/${restaurantId}/menu`,
    )

    return response.data
  } catch {
    return null
  }
}

export async function createMenuForRestaurant(
  restaurantId: number,
  menu: CreateMenuDto,
): Promise<MenuDto> {
  const response = await httpClient.post<MenuDto>(
    `/restaurants/${restaurantId}/menu`,
    menu,
  )

  return response.data
}
export async function updateMenu(
  id: number,
  menu: UpdateMenuDto,
): Promise<void> {
  await httpClient.put(`/menus/${id}`, menu)
}
export async function deleteMenu(id: number): Promise<void> {
  await httpClient.delete(`/menus/${id}`)
}