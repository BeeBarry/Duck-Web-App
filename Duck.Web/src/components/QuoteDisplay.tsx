import React from 'react';
import { RandomQuote } from '../types';

interface QuoteDisplayProps {
    quote: RandomQuote | null;
    isLoading: boolean;
}

const QuoteDisplay: React.FC<QuoteDisplayProps> = ({
                                                       quote,
                                                       isLoading
                                                   }) => {
    if (isLoading) {
        return (
            <div className="p-6 bg-white rounded-lg border-3 border-black shadow-[4px_4px_0px_0px_rgba(0,0,0,0.8)] min-h-48 flex items-center justify-center">
                <p className="text-center">Laddar citat...</p>
            </div>
        );
    }

    if (!quote) {
        return (
            <div className="p-6 bg-white rounded-lg border-3 border-black shadow-[4px_4px_0px_0px_rgba(0,0,0,0.8)] min-h-48 flex items-center justify-center">
                <p className="text-center">
                    Välj en anka och en citattyp, klicka sedan på Quack!
                </p>
            </div>
        );
    }

    return (
        <div className="p-6 bg-white rounded-lg border-3 border-black shadow-[4px_4px_0px_0px_rgba(0,0,0,0.8)] min-h-48 flex flex-col items-center justify-center">
            <p className="text-lg italic text-center mb-6">"{quote.content}"</p>

            <div className="text-center">
                <p className="font-bold mb-2">- {quote.duckName}</p>
                <span className="text-xs px-2 py-1 rounded-full border-2 border-black inline-block">
                    {quote.type}
                </span>
            </div>
        </div>
    );
};

export default QuoteDisplay;