import { useState, useEffect } from 'react';
import './App.css';
import DuckSelector from './components/DuckSelector';
import QuoteTypeSelector from './components/QuoteTypeSelector';
import QuoteDisplay from './components/QuoteDisplay';
import QuackButton from './components/QuackButton';
import { Duck, QuoteType, RandomQuote } from './types';
import { getAllDucks, getRandomQuote } from './services/api';

function App() {
    // Grundläggande tillstånd
    const [ducks, setDucks] = useState<Duck[]>([]);
    const [selectedDuck, setSelectedDuck] = useState<number | null>(null);
    const [selectedQuoteType, setSelectedQuoteType] = useState<QuoteType | null>(null);
    const [currentQuote, setCurrentQuote] = useState<RandomQuote | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);

    // Hämta ankor när komponenten laddas
    useEffect(() => {
        async function fetchDucks() {
            try {
                setIsLoading(true);
                const fetchedDucks = await getAllDucks();
                setDucks(fetchedDucks);
            } catch (error) {
                console.error('Error fetching ducks:', error);
            } finally {
                setIsLoading(false);
            }
        }

        fetchDucks();
    }, []);

    // Hantera val av anka
    const handleDuckSelect = (duckId: number) => {
        setSelectedDuck(duckId);
        setCurrentQuote(null); // Nollställ citatet vid nytt val
    };

    // Hantera val av citattyp
    const handleQuoteTypeSelect = (type: QuoteType) => {
        setSelectedQuoteType(type);
        setCurrentQuote(null); // Nollställ citatet vid nytt val
    };

    // Hämta ett slumpmässigt citat
    const handleQuackClick = async () => {
        if (!selectedDuck || !selectedQuoteType) {
            return;
        }

        try {
            setIsLoading(true);
            const quote = await getRandomQuote(selectedDuck, selectedQuoteType);
            setCurrentQuote(quote);
        } catch (error) {
            console.error('Error fetching quote:', error);
        } finally {
            setIsLoading(false);
        }
    };

    // Kontrollera om quack-knappen ska vara inaktiverad
    const isQuackDisabled = !selectedDuck || !selectedQuoteType;

    return (
        <div className="min-h-screen bg-[#FFB800] py-10 px-4 flex justify-center">
            <div className="max-w-4xl w-full">
                {/* Huvudrubrik - svart text utan ruta */}
                <div className="text-center mb-16">
                    <h1 className="text-5xl font-black text-black">
                        DUCK QUOTES
                    </h1>
                </div>

                {/* Sektion: Välj anka */}
                <div className="mb-12 flex flex-col items-center">
                    <h2 className="text-xl font-bold bg-[#4CAF50] text-white p-6 rounded-lg border-4 border-black mb-6 min-w-[260px] min-h-[50px] text-center flex items-center justify-center">
                        VÄLJ EN ANKA
                    </h2>
                    <DuckSelector
                        ducks={ducks}
                        selectedDuckId={selectedDuck}
                        onSelect={handleDuckSelect}
                    />
                </div>

                {/* Sektion: Välj citattyp */}
                <div className="mb-12 flex flex-col items-center">
                    <h2 className="text-xl font-bold bg-[#2196F3] text-white p-6 rounded-lg border-4 border-black mb-6 min-w-[260px] min-h-[50px] text-center flex items-center justify-center">
                        VÄLJ CITAT TYP
                    </h2>
                    <QuoteTypeSelector
                        selectedType={selectedQuoteType}
                        onSelect={handleQuoteTypeSelect}
                    />
                </div>

                {/* Sektion: Quack-knapp */}
                <div className="mb-12">
                    <QuackButton
                        onClick={handleQuackClick}
                        disabled={isQuackDisabled}
                        isLoading={isLoading}
                    />
                </div>

                {/* Sektion: Citatdisplay */}
                <div>
                    <QuoteDisplay
                        quote={currentQuote}
                        isLoading={isLoading}
                    />
                </div>
            </div>
        </div>
    );
}






export default App;