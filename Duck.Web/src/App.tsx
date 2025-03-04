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
        <div className="min-h-screen w-full bg-[#FFB800] py-8 flex justify-center">
            <div className="w-full max-w-4xl mx-auto px-6">
                <header className="text-center mb-10">
                    <h1 className="text-4xl font-black uppercase mb-3">Duck Quotes</h1>
                    <p className="text-black">Ankor som hjälper dig förstå IT</p>
                </header>

                <main className="bg-white rounded-xl border-3 border-black shadow-[4px_4px_0px_0px_rgba(0,0,0,0.8)] p-8">
                    <div className="mb-10">
                        <h2 className="text-xl font-bold uppercase mb-6">Välj en anka</h2>
                        <DuckSelector
                            ducks={ducks}
                            selectedDuckId={selectedDuck}
                            onSelect={handleDuckSelect}
                        />
                    </div>

                    <div className="mb-10">
                        <h2 className="text-xl font-bold uppercase mb-6">Välj typ av citat</h2>
                        <QuoteTypeSelector
                            selectedType={selectedQuoteType}
                            onSelect={handleQuoteTypeSelect}
                        />
                    </div>

                    {/* Quack-knapp flyttad före citatsektionen */}
                    <div className="mb-10">
                        <QuackButton
                            onClick={handleQuackClick}
                            disabled={isQuackDisabled}
                            isLoading={isLoading}
                        />
                    </div>

                    <div>
                        <h2 className="text-xl font-bold uppercase mb-6">Din ankvisdom</h2>
                        <QuoteDisplay
                            quote={currentQuote}
                            isLoading={isLoading}
                        />
                    </div>
                </main>
            </div>
        </div>
    );
}

export default App;