import { httpClient } from './httpClient'
import type { CreateRestaurantDto, RestaurantDto, UpdateRestaurantDto } from '../types/restaurant'

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
export async function updateRestaurant(
  id: number,
  restaurant: UpdateRestaurantDto,
): Promise<void> {
  await httpClient.put(`/restaurants/${id}`, restaurant)
}