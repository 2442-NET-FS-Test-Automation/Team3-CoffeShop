import { useEffect, useState } from "react";
import "./DashboardPAge.css";
import { api } from "../api/client";
import { AreaChart, Area, XAxis, YAxis, Tooltip, ResponsiveContainer } from "recharts";

    interface TopItems {
        name: string;
        unitsSold: number;
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
        ? Math.max(...stats.topItems.map(item => item.unitsSold)) : 1;



return (
        <div className="dashboard-wrapper">
            <div className="dash-header-row">
                <div className="dash-title">
                    <h1>Analytics</h1>
                    <p>Performance overview · The best coffee of Cognizant</p>
                </div>
            </div>

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
                        <span className="kpi-value">${stats.averageTicket.toFixed(2)}</span>
                    </div>
                </div>
            </div>

            <div className="main-content-grid">

            <div className="dash-panel">
                <div className="dashboard-chart-container">
            <div className="panel-header">
                <h3>Sales by Hour</h3>
                <p>Revenue over time · Today</p>
            </div>

    <ResponsiveContainer width="100%" height="100%">
        <AreaChart 
            data={stats.salesByHour} 
            margin={{ top: 10, right: 10, left: -20, bottom: 50 }}
        >
            <defs>
                <linearGradient id="colorAmount" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#8a5a2f" stopOpacity={0.4}/>
                    <stop offset="95%" stopColor="#8a5a2f" stopOpacity={0}/>
                </linearGradient>
            </defs>
            <XAxis 
                dataKey="hour" 
                stroke="#8a5a2f" 
                fontSize={12} 
                tickLine={false} 
                axisLine={false} 
            />
            <YAxis 
                stroke="#8a5a2f" 
                fontSize={12} 
                tickLine={false} 
                axisLine={false} 
                tickFormatter={(value) => `$${value}`} 
            />
            <Tooltip 
                formatter={(value: any) => {

                    const numericValue = typeof value === 'number' ? value : Number(value) || 0;
                    return [`$${value.toFixed(2)}`, "Revenue"];
                }}
            />
            <Area 
                type="monotone" 
                dataKey="amount" 
                stroke="#2f1b0c" 
                strokeWidth={3}
                fillOpacity={1} 
                fill="url(#colorAmount)" 
            />
        </AreaChart>
    </ResponsiveContainer>
</div>
</div>
 
                <div className="dash-panel">
                    <div className="panel-header">
                        <h3>Top 5 Items</h3>
                        <p>Best-selling products today</p>
                    </div>
                    
                    <div className="top-items-list">
                        {stats.topItems.map((item, index) => {
                    
                            const fillPercentage = (item.unitsSold / maxUnitsSold) * 100;
                            return (
                                <div className="top-item" key={item.name}>
                                    <div className="item-info">
                                        <div className="item-name">
                                            <span className="item-rank">{index + 1}</span>
                                            {item.name}
                                        </div>
                                        <div className="item-units">
                                            <span>{item.unitsSold}</span> units
                                        </div>
                                    </div>
                                    <div className="progress-bar-bg">
                                        <div 
                                            className="progress-bar-fill" 
                                            style={{ width: `${fillPercentage}%` }}
                                        ></div>
                                </div>
                            </div>
                            );
                        })}
                    </div>
                </div>

            </div>
        </div>
    );
};

export default DashboardPage;