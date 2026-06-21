import { httpClient } from './httpClient'
import type { CreateRestaurantDto, RestaurantDto } from '../types/restaurant'

export async function getRestaurants(): Promise<RestaurantDto[]> {
  const response = await httpClient.get<RestaurantDto[]>('/restaurants')

  return response.data
}

export async function createRestaurant(
  restaurant: CreateRestaurantDto,
): Promise<RestaurantDto> {
  const response = await httpClient.post<RestaurantDto>('/restaurants', restaurant)

  return response.data
}
export async function deleteRestaurant(id: number): Promise<void> {
  await httpClient.delete(`/restaurants/${id}`)
}