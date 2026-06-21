import { useEffect, useState } from 'react'
import './App.css'
import { getRestaurants } from './api/restaurantApi'
import type { RestaurantDto } from './types/restaurant'

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

function App() {
  const [restaurants, setRestaurants] = useState<RestaurantDto[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [errorMessage, setErrorMessage] = useState<string | null>(null)

  useEffect(() => {
    async function loadRestaurants() {
      try {
        const data = await getRestaurants()
        setRestaurants(data)
      } catch {
        setErrorMessage('Failed to load restaurants.')
      } finally {
        setIsLoading(false)
      }
    }

    loadRestaurants()
  }, [])

  return (
    <main className="app">
      <section className="card">
        <h1>Restaurant Network</h1>
        <p className="subtitle">
          React + TypeScript client connected to ASP.NET Core Web API.
        </p>

        {isLoading && <p>Loading restaurants...</p>}

        {errorMessage && <p className="error">{errorMessage}</p>}

        {!isLoading && !errorMessage && (
          <div className="restaurant-list">
            {restaurants.map((restaurant) => (
              <article key={restaurant.id} className="restaurant-card"  style={{ borderLeftColor: getRestaurantColor(restaurant.color) }}>
                <h2 style={{ color: getRestaurantColor(restaurant.color) }}>
                  {restaurant.color} Restaurant
                </h2>
                <p>    <strong>Address:</strong> {restaurant.address}  </p>
                <p>    <strong>Rent:</strong> {restaurant.rent} </p>
                </article>))}
          </div>
        )}
      </section>
    </main>
  )
}

export default App