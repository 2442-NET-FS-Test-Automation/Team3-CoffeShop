import { Route, Routes } from 'react-router-dom'
import './App.css'
import Navbar from './Components/Navbar/Navbar.tsx'
import MenuPage from './Pages/MenuPage.tsx'
import LoginPage from './Pages/LoginPage.tsx'
import RegisterPage from './Pages/RegisterPage.tsx'
import { AuthProvider } from './auth/AuthContext.tsx'
//import OrderPage from './Pages/OrderPage.tsx'
import InventoryPage from './Pages/InventoryPage.tsx'
import DashboardPage from './Pages/DashboradPage.tsx'

function App() {
  return (
    <AuthProvider>
    <Routes>
              <Route path='/' element={<LoginPage/>} />
              <Route path='/register' element={<RegisterPage/>} />

              <Route element={<Navbar/>}>
              <Route path = '/menu' element={<MenuPage/>} />
              <Route path='/inventory' element={<InventoryPage/>} />
              <Route path='/dashboard' element={<DashboardPage/>} />
              </Route>
      </Routes>
      </AuthProvider>
  )
}

export default App
