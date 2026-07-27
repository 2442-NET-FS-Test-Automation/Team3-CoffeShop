import { useEffect, useState } from "react";
import "./DashboardPAge.css";
import { api } from "../api/client";

    interface TopItems {
        name: string;
        unitSold: number;
    }

    interface HourlySale {
        hour: string;
        amount: number;
    }

    interface AnalitycsDashboard   {
          totalOrders: number;
          totalRevenue: number;
          averageTicket: number;
          topItems: TopItems[];
          salesByHour: HourlySale[];
          revenueTrend: number;
          ticketTrend: number;
          ordersTrend: number

    }


const DashboardPage = () => { 

    const [stats, setStats] = useState<AnalitycsDashboard | null>(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {

        const fetchDashboardData = async () => {

            try{

                const respone = await api.get<AnalitycsDashboard>("/api/Analytics/Analytics");
                setStats(respone.data);
            }
            catch(error)
            {
                
                console.error("Error while charging the dashboard:", error);

            }finally{
                setIsLoading(false);
            }
        };

            fetchDashboardData();


    }, []);

    if (isLoading){

        return(

            <div className="dashboard-wrapper">
                <div className="dash-header-row">
                    <h1>Charging Data...</h1>
                </div>
            </div>
        );
    }

    if(!stats){
        return (
                <div className="dashboard-wrapper">
                <div className="dash-header-row">
                    <h1>Error while charging the data from db</h1>
                </div>
            </div>
        );
    }

    const maxUnitsSold = stats.topItems.length > 0
        ? Math.max(...stats.topItems.map(item => item.unitSold)) : 1;



return (
        <div className="dashboard-wrapper">
            {/* ENCABEZADO */}
            <div className="dash-header-row">
                <div className="dash-title">
                    <h1>Analytics</h1>
                    <p>Performance overview · The best coffee of Cognizant</p>
                </div>
            </div>

            {/* TARJETAS KPI */}
            <div className="kpi-grid">
                <div className="kpi-card">
                    <div className="kpi-info">
                        <span className="kpi-title">Total Revenue</span>
                        <span className="kpi-value">${stats.totalRevenue.toLocaleString()}</span>
                    </div>
                </div>

                <div className="kpi-card">
                    <div className="kpi-info">
                        <span className="kpi-title">Total Orders</span>
                        <span className="kpi-value">{stats.totalOrders}</span>
                    </div>
                </div>

                <div className="kpi-card">
                    <div className="kpi-info">
                        <span className="kpi-title">Average Ticket</span>
                        <span className="kpi-value">${stats.averageTicket}</span>
                    </div>
                </div>
            </div>

            {/* CONTENIDO PRINCIPAL */}
            <div className="main-content-grid">
                
                {/* Panel Izquierdo: Gráfica */}
                <div className="dash-panel">
                    <div className="panel-header">
                        <h3>Sales by Hour</h3>
                        <p>Revenue over time · Today</p>
                    </div>
                    <div className="chart-placeholder">
                        <p>Gráfica funcional pendiente de conexión 📈</p>
                    </div>
                </div>

                {/* Panel Derecho: Top 5 */}
                <div className="dash-panel">
                    <div className="panel-header">
                        <h3>Top 5 Items</h3>
                        <p>Best-selling products today</p>
                    </div>
                    
                    <div className="top-items-list">
                        {stats.topItems.map((item, index) => (
                            <div className="top-item" key={item.name}>
                                <div className="item-info">
                                    <div className="item-name">
                                        <span className="item-rank">{index + 1}</span>
                                        {item.name}
                                    </div>
                                    <div className="item-units">
                                        <span>{item.unitSold}</span> units
                                    </div>
                                </div>
                                <div className="progress-bar-bg">
                                    <div 
                                        className="progress-bar-fill" 
                                        style={{ width: `${item.name}%` }}
                                    ></div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>

            </div>
        </div>
    );
};

export default DashboardPage;