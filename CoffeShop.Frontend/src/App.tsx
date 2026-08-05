import { Navigate, Route, Routes } from 'react-router-dom'
import './App.css'
import MenuPage from './Pages/MenuPage.tsx'
import LoginPage from './Pages/LoginPage.tsx'
import RegisterPage from './Pages/RegisterPage.tsx'
import InventoryPage from './Pages/InventoryPage.tsx'
import DashboardPage from './Pages/DashboradPage.tsx'
import RequireAuth from './auth/RequireAuth.tsx'
import ProtectedLayout from './Components/ProtectedLayout/ProtectedLayout.tsx'

function App() {
  return ( 
     <Routes>
        <Route path='/' element={<Navigate to="/login" replace />} />
        <Route path='/login' element ={<LoginPage/>}/>
        <Route path='/register' element ={<RegisterPage/>}/>

        <Route element={<ProtectedLayout />}>
          <Route 
            path = "/menu"
            element = {
              <RequireAuth allowedRoles ={["Manager", "Barista"]}>
                <MenuPage/>
              </RequireAuth>
            } 
          />
          <Route 
            path = "/dashboard"
            element = {
              <RequireAuth allowedRoles ={["Manager"]}>
                <DashboardPage/>
              </RequireAuth>
            } 
          />
          <Route 
            path = "/inventory"
            element = {
              <RequireAuth allowedRoles ={["Manager", "Barista"]}>
                <InventoryPage/>
              </RequireAuth>
            } 
          />
        </Route>

        <Route path='*' element={<Navigate to="/menu" replace />} />
      </Routes>
  )
}

export default App
