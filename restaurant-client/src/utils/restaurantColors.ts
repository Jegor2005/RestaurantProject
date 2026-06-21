export function getRestaurantColor(color: string): string {
  switch (color.toLowerCase()) {
    case 'red':
      return '#dc2626'
    case 'blue':
      return '#2563eb'
    case 'green':
      return '#16a34a'
    case 'yellow':
      return '#ca8a04'
    default:
      return '#1f2937'
  }
}