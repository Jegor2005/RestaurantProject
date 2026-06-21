import { useEffect, useState } from 'react'
import type { FormEvent } from 'react'
import './App.css'
import {createRestaurant,  deleteRestaurant,  getRestaurants} from './api/restaurantApi'
import type { CreateRestaurantDto, RestaurantDto } from './types/restaurant'

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

const initialFormState: CreateRestaurantDto = {
  color: '',
  address: '',
  rent: 0,
}

function App() {
  const [restaurants, setRestaurants] = useState<RestaurantDto[]>([])
  const [formData, setFormData] = useState<CreateRestaurantDto>(initialFormState)
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

    if (!formData.color.trim() || !formData.address.trim() || formData.rent <= 0) {
      setErrorMessage('Please fill in all fields. Rent must be greater than 0.')
      return
    }

    try {
      setIsSubmitting(true)
      setErrorMessage(null)

      const createdRestaurant = await createRestaurant({
        color: formData.color.trim(),
        address: formData.address.trim(),
        rent: formData.rent,
      })

      setRestaurants((currentRestaurants) => [
        ...currentRestaurants,
        createdRestaurant,
      ])
      setFormData(initialFormState)
    } catch {
      setErrorMessage('Failed to create restaurant.')
    } finally {
      setIsSubmitting(false)
    }
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
          <h2>Add restaurant</h2>

          <div className="form-grid">
            <label>
              Color
              <input
                value={formData.color}
                onChange={(event) =>
                  setFormData({
                    ...formData,
                    color: event.target.value,
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
                    rent: Number(event.target.value),
                  })
                }
              />
            </label>
          </div>

          <button type="submit" disabled={isSubmitting}>
            {isSubmitting ? 'Adding...' : 'Add restaurant'}
          </button>
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
                
                  <button type="button" className="delete-button" onClick={() => handleDelete(restaurant.id)}>  Delete </button>
              </article>
            ))}
          </div>
        )}
      </section>
    </main>
  )
}

export default App