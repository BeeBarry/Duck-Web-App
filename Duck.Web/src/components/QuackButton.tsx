import React from 'react';

interface QuackButtonProps {
    onClick: () => void;
    disabled: boolean;
    isLoading: boolean;
}

const QuackButton: React.FC<QuackButtonProps> = ({
                                                     onClick,
                                                     disabled,
                                                     isLoading
                                                 }) => {
    return (
        <button
            onClick={onClick}
            disabled={disabled || isLoading}
            className={`
        w-full py-4 px-6 rounded-lg text-xl font-bold
        border-3 border-black
        shadow-[4px_4px_0px_0px_rgba(0,0,0,0.8)]
        focus:outline-none
        ${disabled
                ? 'bg-gray-300 text-gray-600 cursor-not-allowed'
                : 'bg-[#FF3B3B] text-white hover:bg-[#FF2525]'
            }
      `}
        >
            {isLoading ? (
                <span>Quacking...</span>
            ) : (
                <div className="flex items-center justify-center gap-2">
                    <span>🦆</span>
                    <span>QUACK!</span>
                </div>
            )}
        </button>
    );
};

export default QuackButton;