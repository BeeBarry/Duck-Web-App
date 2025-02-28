import { Duck, QuoteType, RandomQuote} from '../types';


// URL för API
const API_BASE_URL = 'http://localhost:5115/api';

// Hämta alla ankor och visa i UI
export async function getAllDucks(): Promise<Duck[]> {
    try {
        const response = await fetch(`${API_BASE_URL}/user/ducks`);

        if (!response.ok) {
            throw new Error(`Fel vid hämtning av ankor: ${response.status}`);
        }

        return await response.json();
    } catch(error) {
        console.error('Kunde inte hämta ankor:', error);
        throw error;
    }
}

// Hämta quote från vald anka och en av dess valda quote types
export async function getRandomQuote(duckId: number, quoteType: QuoteType): Promise<RandomQuote> {
    try {
        const response = await fetch(
            `${API_BASE_URL}/user/ducks/${duckId}/quotes/random?type=${quoteType}`
        );

        if (!response.ok) {
            throw new Error(`Fel vid hämtning av citat: ${response.status}`);
        }

        return await response.json();
    } catch(error) {
        console.error('Kunde inte hämta citat:', error);
        throw error;
    }
}