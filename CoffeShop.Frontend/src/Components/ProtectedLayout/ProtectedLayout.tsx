import { Outlet } from 'react-router-dom'
import Navbar from '../Navbar/Navbar'

function ProtectedLayout() {
    return (
        <>
            <Navbar />
            <main className="app">
                <Outlet />
            </main>
        </>
    )
}

export default ProtectedLayout
