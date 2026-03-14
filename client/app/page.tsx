'use client'

import { useState, useEffect } from "react";
import { getMonthlyOrderStats } from "../lib/api";
import { Chart, registerables } from 'chart.js';
import {Bar} from "react-chartjs-2";

Chart.register(...registerables);

export default function Home() {
  const [monthlyStats, setMonthlyStats] = useState<{ CompanyName: string; TotalIncome: number }[] | null>(null);

  const labels = monthlyStats?.map((stat) => stat.CompanyName) || [];
  const datasets = monthlyStats?.map((stat) => Number(stat.TotalIncome)) || [];
  const data = {
    labels: labels,
    datasets: [
      {
        // Title of Graph
        label: "My Bar Chart",
        data: datasets,
        backgroundColor: [
          "rgba(255, 99, 132, 0.2)",
          "rgba(255, 159, 64, 0.2)",
          "rgba(255, 205, 86, 0.2)",
          "rgba(75, 192, 192, 0.2)",
        ],
        borderColor: [
          "rgb(255, 99, 132)",
          "rgb(255, 159, 64)",
          "rgb(255, 205, 86)",
          "rgb(75, 192, 192)",
        ],
        borderWidth: 1,
        barPercentage: 1,
        borderRadius: {
          topLeft: 5,
          topRight: 5,
        },
      },
      // insert similar in dataset object for making multi bar chart
    ],
  };
  const options = {
    scales: {
      y: {
        title: {
          display: true,
          text: "Total Income",
        },
        display: true,
        beginAtZero: true,
        max: 9999.99,
      },
      x: {
        title: {
          display: true,
          text: "Companies",
        },
        display: true,
      },
    },
  };

  useEffect(() => {
    getMonthlyOrderStats()
      .then(setMonthlyStats)
      .catch(error => {
        console.error('Error fetching monthly order stats:', error);
      });
  }, []);

  return (
    <div className="flex min-h-screen items-center justify-center bg-zinc-50 font-sans dark:bg-black">
      <main className="flex min-h-screen w-full max-w-3xl flex-col items-center justify-between py-32 px-16 bg-white dark:bg-black sm:items-start">
        <div className="mt-8 w-full max-w-md">
          <h2 className="text-xl font-semibold mb-4 text-black dark:text-zinc-50">Chart.js Example</h2>
              <div style={{ width: "1000px" }}>
              <Bar data={data} options={options} />
              </div>
        </div>
      </main>
    </div>
  );
}
