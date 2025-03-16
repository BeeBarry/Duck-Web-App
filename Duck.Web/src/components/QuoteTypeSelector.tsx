import React from 'react';
import { QuoteType } from '../types';

interface QuoteTypeSelectorProps {
    selectedType: QuoteType | null;
    onSelect: (type: QuoteType) => void;
}

const QuoteTypeSelector: React.FC<QuoteTypeSelectorProps> = ({
                                                                 selectedType,
                                                                 onSelect
                                                             }) => {
    // Testar emojis som design
    const typeConfig = {
        [QuoteType.Wise]: {
            icon: "🧠",
            label: "WISE"
        },
        [QuoteType.Comical]: {
            icon: "😂",
            label: "COMICAL"
        },
        [QuoteType.Dark]: {
            icon: "🖤",
            label: "DARK"
        }
    };

    return (
        <div className="grid grid-cols-3 gap-4 md:gap-6">
            {Object.values(QuoteType).map((type) => (
                <button
                    key={type}
                    onClick={() => onSelect(type)}
                    className={`
                        py-4 px-2
                        font-bold text-black
                        transform transition-all duration-200
                        border-3 border-black
                        shadow-[4px_4px_0px_0px_rgba(0,0,0,0.8)]
                        ${selectedType === type
                        ? 'bg-[#E0E0E0] translate-x-[2px] translate-y-[2px] shadow-[2px_2px_0px_0px_rgba(0,0,0,0.8)]'
                        : 'bg-white hover:translate-y-1 hover:translate-x-1 hover:shadow-[2px_2px_0px_0px_rgba(0,0,0,0.8)]'
                    }
                    `}
                    style={{ fontFamily: "Courier New, monospace" }}
                >
                    <div className="flex flex-col items-center justify-center">
                        <span className="text-2xl mb-2">{typeConfig[type].icon}</span>
                        <span>{typeConfig[type].label}</span>
                    </div>
                </button>
            ))}
        </div>
    );
};

export default QuoteTypeSelector;