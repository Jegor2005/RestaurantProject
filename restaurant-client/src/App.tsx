import { useEffect, useState } from 'react'
import type { FormEvent } from 'react'
import './App.css'
import {
  createMenuForRestaurant,
  getMenuByRestaurantId,
} from './api/menuApi'
import {
  createRestaurant,
  deleteRestaurant,
  getRestaurants,
  updateRestaurant,
} from './api/restaurantApi'
import type { MenuDto } from './types/menu'
import {
  initialMenuFormState,
  type MenuFormState,
} from './types/menuForm'
import {
  initialRestaurantFormState,
  type RestaurantFormState,
} from './types/restaurantForm'
import type { RestaurantDto } from './types/restaurant'
import { getRestaurantColor } from './utils/restaurantColors'

function App() {
  const [restaurants, setRestaurants] = useState<RestaurantDto[]>([])
  const [formData, setFormData] = useState<RestaurantFormState>(
    initialRestaurantFormState,
  )
  const [editingRestaurantId, setEditingRestaurantId] = useState<number | null>(
    null,
  )
  const [isLoading, setIsLoading] = useState(true)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [errorMessage, setErrorMessage] = useState<string | null>(null)

  const [selectedRestaurantId, setSelectedRestaurantId] = useState<number | null>(
    null,
  )
  const [selectedMenu, setSelectedMenu] = useState<MenuDto | null>(null)
  const [isMenuLoading, setIsMenuLoading] = useState(false)
  const [menuFormData, setMenuFormData] =
    useState<MenuFormState>(initialMenuFormState)
  const [isMenuSubmitting, setIsMenuSubmitting] = useState(false)

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

    const rentValue = Number(formData.rent)

    if (!formData.color.trim() || !formData.address.trim() || rentValue <= 0) {
      setErrorMessage('Please fill in all fields. Rent must be greater than 0.')
      return
    }

    try {
      setIsSubmitting(true)
      setErrorMessage(null)

      const restaurantData = {
        color: formData.color.trim(),
        address: formData.address.trim(),
        rent: rentValue,
      }

      if (editingRestaurantId !== null) {
        await updateRestaurant(editingRestaurantId, restaurantData)

        setRestaurants((currentRestaurants) =>
          currentRestaurants.map((restaurant) =>
            restaurant.id === editingRestaurantId
              ? {
                  ...restaurant,
                  ...restaurantData,
                }
              : restaurant,
          ),
        )

        setEditingRestaurantId(null)
      } else {
        const createdRestaurant = await createRestaurant(restaurantData)

        setRestaurants((currentRestaurants) => [
          ...currentRestaurants,
          createdRestaurant,
        ])
      }

      setFormData(initialRestaurantFormState)
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
    setFormData(initialRestaurantFormState)
    setErrorMessage(null)
  }

  async function handleViewMenu(restaurantId: number) {
    try {
      setIsMenuLoading(true)
      setErrorMessage(null)
      setSelectedRestaurantId(restaurantId)
      setMenuFormData(initialMenuFormState)

      const menu = await getMenuByRestaurantId(restaurantId)

      setSelectedMenu(menu)
    } catch {
      setErrorMessage('Failed to load menu.')
    } finally {
      setIsMenuLoading(false)
    }
  }

  async function handleCreateMenu(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()

    if (selectedRestaurantId === null) {
      return
    }

    if (!menuFormData.name.trim()) {
      setErrorMessage('Menu name is required.')
      return
    }

    try {
      setIsMenuSubmitting(true)
      setErrorMessage(null)

      const createdMenu = await createMenuForRestaurant(selectedRestaurantId, {
        name: menuFormData.name.trim(),
        description: menuFormData.description.trim() || null,
      })

      setSelectedMenu(createdMenu)
      setMenuFormData(initialMenuFormState)
    } catch {
      setErrorMessage('Failed to create menu.')
    } finally {
      setIsMenuSubmitting(false)
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

      if (selectedRestaurantId === id) {
        setSelectedRestaurantId(null)
        setSelectedMenu(null)
      }
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
          <h2>
            {editingRestaurantId === null ? 'Add restaurant' : 'Edit restaurant'}
          </h2>

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
                    rent: event.target.value,
                  })
                }
                placeholder="1100"
              />
            </label>
          </div>

          <button type="submit" disabled={isSubmitting}>
            {isSubmitting
              ? 'Saving...'
              : editingRestaurantId === null
                ? 'Add restaurant'
                : 'Save changes'}
          </button>

          {editingRestaurantId !== null && (
            <button
              type="button"
              className="secondary-button"
              onClick={handleCancelEdit}
            >
              Cancel
            </button>
          )}
        </form>

        {isLoading && <p>Loading restaurants...</p>}

        {errorMessage && <p className="error">{errorMessage}</p>}

        {!isLoading && (
          <>
            <div className="restaurant-list">
              {restaurants.map((restaurant) => (
                <article
                  key={restaurant.id}
                  className="restaurant-card"
                  style={{
                    borderLeftColor: getRestaurantColor(restaurant.color),
                  }}
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
                    <button
                      type="button"
                      className="action-button view-button"
                      onClick={() => handleViewMenu(restaurant.id)}
                    >
                      View menu
                    </button>

                    <button
                      type="button"
                      className="action-button edit-button"
                      onClick={() => handleEdit(restaurant)}
                    >
                      Edit
                    </button>

                    <button
                      type="button"
                      className="action-button delete-button"
                      onClick={() => handleDelete(restaurant.id)}
                    >
                      Delete
                    </button>
                  </div>
                </article>
              ))}
            </div>

            {selectedRestaurantId !== null && (
              <section className="menu-section">
                <h2>Selected Restaurant Menu</h2>

                {isMenuLoading && <p>Loading menu...</p>}

                {!isMenuLoading && selectedMenu && (
                  <div className="menu-card">
                    <h3>{selectedMenu.name}</h3>

                    <p>{selectedMenu.description ?? 'No description provided.'}</p>

                    <p>
                      <strong>Restaurant ID:</strong>{' '}
                      {selectedMenu.restaurantId}
                    </p>
                  </div>
                )}

                {!isMenuLoading && !selectedMenu && (
                  <>
                    <p className="empty-message">
                      No menu found for this restaurant.
                    </p>

                    <form className="menu-form" onSubmit={handleCreateMenu}>
                      <h3>Create menu</h3>

                      <label>
                        Name
                        <input
                          value={menuFormData.name}
                          onChange={(event) =>
                            setMenuFormData({
                              ...menuFormData,
                              name: event.target.value,
                            })
                          }
                          placeholder="Main Menu"
                        />
                      </label>

                      <label>
                        Description
                        <textarea
                          value={menuFormData.description}
                          onChange={(event) =>
                            setMenuFormData({
                              ...menuFormData,
                              description: event.target.value,
                            })
                          }
                          placeholder="Default menu for this restaurant"
                        />
                      </label>

                      <button type="submit" disabled={isMenuSubmitting}>
                        {isMenuSubmitting ? 'Creating...' : 'Create menu'}
                      </button>
                    </form>
                  </>
                )}
              </section>
            )}
          </>
        )}
      </section>
    </main>
  )
}

export default App