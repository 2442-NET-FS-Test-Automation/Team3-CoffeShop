import './App.css'
import Navbar from './Components/Navbar/Navbar.tsx'
import LoginPage from './Pages/LoginPage.tsx'

function App() {
  return (
      <div className='App'>
        <Navbar />
        <main className="app">
        <LoginPage />
        </main>
      </div>
      
  )
}

export default App
