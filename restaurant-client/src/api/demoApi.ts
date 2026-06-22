import { httpClient } from './httpClient'

export async function resetDemoData(): Promise<void> {
  await httpClient.post('/demo/reset')
}