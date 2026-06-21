export interface RestaurantDto {
  id: number
  color: string
  address: string
  rent: number
}

export interface CreateRestaurantDto {
  color: string
  address: string
  rent: number
}

export interface UpdateRestaurantDto {
  color: string
  address: string
  rent: number
}