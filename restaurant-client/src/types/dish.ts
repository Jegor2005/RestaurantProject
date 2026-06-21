export interface DishDto {
  id: number
  name: string
  price: number
  category: string
  description: string
  menuId: number
}

export interface CreateDishDto {
  name: string
  price: number
  category: string
  description: string
}