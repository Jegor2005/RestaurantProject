import { useEffect, useState } from 'react'
import type { FormEvent } from 'react'
import './App.css'
import {
  createRestaurant,
  deleteRestaurant,
  getRestaurants,
  updateRestaurant,
} from './api/restaurantApi'
import type { CreateRestaurantDto, RestaurantDto} from './types/restaurant'

function getRestaurantColor(color: string): string {
  switch (color.toLowerCase()) {
    case 'red':
      return '#dc2626'
    case 'blue':
      return '#2563eb'
    case 'green':
      return '#16a34a'
    default:
      return '#1f2937'
  }
}
type RestaurantFormState = {
  color: string
  address: string
  rent: string
}
const initialFormState: RestaurantFormState = {
  color: '',
  address: '',
  rent: '',
}

function App() {
  const [restaurants, setRestaurants] = useState<RestaurantDto[]>([])
  const [formData, setFormData] = useState<RestaurantFormState>(initialFormState)
  const [editingRestaurantId, setEditingRestaurantId] = useState<number | null>(null)
  const [isLoading, setIsLoading] = useState(true)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [errorMessage, setErrorMessage] = useState<string | null>(null)

  useEffect(() => {
    loadRestaurants()
  }, [])

  async function loadRestaurants() {
    try {
      setErrorMessage(null)
      const data = await getRestaurants()
      setRestaurants(data)
    } catch {
      setErrorMessage('Failed to load restaurants.')
    } finally {
      setIsLoading(false)
    }
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
  event.preventDefault()

  if (!formData.color.trim() || !formData.address.trim() || !formData.rent.trim()) {
    setErrorMessage('Please fill in all fields. Rent must be greater than 0.')
    return
  }

  try {
    setIsSubmitting(true)
    setErrorMessage(null)

    
    const rentValue = Number(formData.rent)
    const restaurantData = {
      color: formData.color.trim(),
      address: formData.address.trim(),
      rent: rentValue,
    }

    
    if (!formData.color.trim() || !formData.address.trim() || rentValue <= 0) {
      setErrorMessage('Please fill in all fields. Rent must be greater than 0.')
      return
    } else {
      const createdRestaurant = await createRestaurant(restaurantData)

      setRestaurants((currentRestaurants) => [
        ...currentRestaurants,
        createdRestaurant,
      ])
    }

    setFormData(initialFormState)
  } catch {
    setErrorMessage(
      editingRestaurantId !== null
        ? 'Failed to update restaurant.'
        : 'Failed to create restaurant.',
    )
  } finally {
    setIsSubmitting(false)
  }
}
function handleEdit(restaurant: RestaurantDto) {
  setEditingRestaurantId(restaurant.id)
  setFormData({
    color: restaurant.color,
    address: restaurant.address,
    rent: String(restaurant.rent),
  })
  setErrorMessage(null)
}
function handleCancelEdit() {
  setEditingRestaurantId(null)
  setFormData(initialFormState)
  setErrorMessage(null)
}
  async function handleDelete(id: number) {
  const shouldDelete = window.confirm('Delete this restaurant?')

  if (!shouldDelete) {
    return
  }

  try {
    setErrorMessage(null)

    await deleteRestaurant(id)

    setRestaurants((currentRestaurants) =>
      currentRestaurants.filter((restaurant) => restaurant.id !== id),
    )
  } catch {
    setErrorMessage('Failed to delete restaurant.')
  }
}


  return (
    <main className="app">
      <section className="card">
        <h1>Restaurant Network</h1>
        <p className="subtitle">
          React + TypeScript client connected to ASP.NET Core Web API.
        </p>

        <form className="restaurant-form" onSubmit={handleSubmit}>
          <h2>{editingRestaurantId === null ? 'Add restaurant' : 'Edit restaurant'}</h2>

          <div className="form-grid">
            <label>
              Color
              <input
                value={formData.color}
                onChange={(event) =>
                setFormData({
                  ...formData,
                  rent: event.target.value
                })
}
                placeholder="Yellow"
              />
            </label>

            <label>
              Address
              <input
                value={formData.address}
                onChange={(event) =>
                  setFormData({
                    ...formData,
                    address: event.target.value,
                  })
                }
                placeholder="Koper, Slovenia"
              />
            </label>

            <label>
              Rent
              <input
                type="number"
                min="1"
                value={formData.rent}
                onChange={(event) =>
                  setFormData({
                    ...formData,
                    rent: event.target.value,
                  })
                }
              />
            </label>
          </div>

          <button type="submit" disabled={isSubmitting}>
              {isSubmitting  ? 'Saving...' : editingRestaurantId === null ? 'Add restaurant' : 'Save changes'}
          </button>
          {editingRestaurantId !== null && (
            <button type="button" className="secondary-button" onClick={handleCancelEdit}>
              Cancel
            </button>)}
        </form>

        {isLoading && <p>Loading restaurants...</p>}

        {errorMessage && <p className="error">{errorMessage}</p>}

        {!isLoading && !errorMessage && (
          <div className="restaurant-list">
            {restaurants.map((restaurant) => (
              <article
                key={restaurant.id}
                className="restaurant-card"
                style={{ borderLeftColor: getRestaurantColor(restaurant.color) }}
              >
                <h2 style={{ color: getRestaurantColor(restaurant.color) }}>
                  {restaurant.color} Restaurant
                </h2>
                <p>
                  <strong>Address:</strong> {restaurant.address}
                </p>
                <p>
                  <strong>Rent:</strong> {restaurant.rent}
                </p>
                <div className="card-actions">
                  <button  type="button"  className="edit-button"  onClick={() => handleEdit(restaurant)}>  Edit  </button>
                  <button type="button" className="delete-button" onClick={() => handleDelete(restaurant.id)}>  Delete </button>
                </div>
              </article>
            ))}
          </div>
        )}
      </section>
    </main>
  )
}

export default App