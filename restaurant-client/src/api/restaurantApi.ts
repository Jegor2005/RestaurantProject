import { httpClient } from './httpClient'
import type { RestaurantDto } from '../types/restaurant'

export async function getRestaurants(): Promise<RestaurantDto[]> {
  const response = await httpClient.get<RestaurantDto[]>('/restaurants')

  return response.data
}