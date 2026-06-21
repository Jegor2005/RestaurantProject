import { httpClient } from './httpClient'
import type { MenuDto } from '../types/menu'

export async function getMenuByRestaurantId(
  restaurantId: number,
): Promise<MenuDto | null> {
  try {
    const response = await httpClient.get<MenuDto>(
      `/restaurants/${restaurantId}/menu`,
    )

    return response.data
  } catch (error) {
    return null
  }
}