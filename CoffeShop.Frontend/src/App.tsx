import './App.css'
import Navbar from './Components/Navbar/Navbar.tsx'
import MenuPage from './Pages/MenuPage.tsx'

function App() {
  return (
      <div className='App'>
        <Navbar />
        <main className="app">
          <MenuPage />
        </main>
      </div>
  )
}

export default App
