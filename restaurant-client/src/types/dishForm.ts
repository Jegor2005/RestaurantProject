export type DishFormState = {
  name: string
  price: string
  category: string
  description: string
}

export const initialDishFormState: DishFormState = {
  name: '',
  price: '',
  category: '',
  description: '',
}