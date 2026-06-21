export interface MenuDto {
  id: number
  name: string
  description: string | null
  restaurantId: number
}

export interface CreateMenuDto {
  name: string
  description?: string | null
}

export interface UpdateMenuDto {
  name: string
  description?: string | null
}