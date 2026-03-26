'use client';

import { useState, useEffect } from "react";
import { getMonthlyOrderStats, getMonthlyIncomeSummary, getOrders } from "../lib/api";
import { Chart, registerables } from 'chart.js';
import {Bar} from "react-chartjs-2";
import { getBarChartOptions } from "../lib/chartConfig";
import { chartColors } from "../utils/chartColors";
import { Card } from "../components/ui/Card";
import { ChartContainer } from "../components/ui/ChartContainer";

Chart.register(...registerables);

export default function Home() {
  const [monthlyStats, setMonthlyStats] = useState<{ CompanyName: string; TotalIncome: number; TotalOrders: number; Year: number; Month: number; CompanyId: number }[] | null>(null);
  const [monthlyIncomeSummary, setMonthlyIncomeSummary] = useState<{ Year: number; Month: number; TotalIncome: number }[] | null>(null);

  const labels = monthlyStats?.map((stat) => stat.CompanyName) || [];
  const datasets = monthlyStats?.map((stat) => Number(stat.TotalIncome)) || [];
  
  // Use green/earth color palette for company income chart
  const companyIncomeData = {
    labels: labels,
    datasets: [
      {
        label: "Total Income",
        data: datasets,
        backgroundColor: chartColors.primary,
        borderColor: chartColors.primaryDark,
        borderWidth: 1,
        barPercentage: 0.8,
        borderRadius: 8,
      },
    ],
  };
  
  const companyIncomeOptions = getBarChartOptions("Company Income Overview", "Income ($)");

  // Month labels for the income summary chart
  const monthLabels = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
  
  // Prepare data for monthly income summary chart
  const incomeSummaryDatasets = monthlyIncomeSummary?.map((item) => Number(item.TotalIncome)) || [];
  const incomeSummaryData = {
    labels: monthLabels,
    datasets: [
      {
        label: "Total Income",
        data: incomeSummaryDatasets,
        backgroundColor: chartColors.primary,
        borderColor: chartColors.primaryDark,
        borderWidth: 1,
        barPercentage: 0.8,
        borderRadius: 8,
      },
    ],
  };
  
  const incomeSummaryOptions = getBarChartOptions("Monthly Income Summary", "Income ($)");

  useEffect(() => {
    getMonthlyOrderStats()
      .then(setMonthlyStats)
      .catch(error => {
        console.error('Error fetching monthly order stats:', error);
      });
    
    getMonthlyIncomeSummary()
      .then(setMonthlyIncomeSummary)
      .catch(error => {
        console.error('Error fetching monthly income summary:', error);
      });
    
    getOrders()
      .catch(error => {
        console.error('Error fetching orders:', error);
      });
  }, []);

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white border-b border-gray-200 sticky top-0 z-10">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6">
          <div>
            <h1 className="text-2xl font-bold text-gray-900">Logistics Visualization Dashboard</h1>
            <p className="text-sm text-gray-500 mt-1">Real-time data insights and analytics</p>
          </div>
        </div>
      </header>
      
      {/* Main Content */}
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
          {/* Company Income Chart */}
          <Card className="col-span-1 lg:col-span-2">
            <ChartContainer title="Company Income Overview" description="Monthly income breakdown by company">
              <div style={{ height: '400px' }}>
                <Bar data={companyIncomeData} options={companyIncomeOptions} />
              </div>
            </ChartContainer>
          </Card>
          
          {/* Monthly Income Summary Chart */}
          <Card className="col-span-1 lg:col-span-2">
            <ChartContainer title="Monthly Income Summary" description="Yearly income trends across all months">
              <div style={{ height: '400px' }}>
                <Bar data={incomeSummaryData} options={incomeSummaryOptions} />
              </div>
            </ChartContainer>
          </Card>
        </div>
      </main>
    </div>
  );
}
