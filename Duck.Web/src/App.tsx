/*import { useState } from 'react' */
import './App.css'

function App() {
  /*const [count, setCount] = useState(0)*/

    return (
        <div className="min-h-screen bg-gray-100 flex items-center justify-center">
            <div className="bg-white p-8 rounded-lg shadow-md max-w-md">
                <h1 className="text-3xl font-bold text-blue-600 mb-4">
                    Duck Quotes
                </h1>
                <p className="text-gray-600">
                    Välj en anka för att se ett citat!
                </p>
            </div>
        </div>
    )
}

export default App

