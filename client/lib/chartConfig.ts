import { ChartOptions } from 'chart.js';

export const getBarChartOptions = (title: string, subtitle: string): ChartOptions<'bar'> => ({
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      position: 'top',
      align: 'start',
      labels: {
        usePointStyle: true,
        boxWidth: 8,
        font: {
          size: 12,
          family: 'Inter, system-ui, sans-serif',
        },
      },
    },
  },
  scales: {
    y: {
      beginAtZero: true,
      grid: {
        color: '#f3f4f6',
      },
      ticks: {
        color: '#6b7280',
        font: {
          size: 11,
        },
        callback: (value: string | number) => {
          if (typeof value === 'number') {
            return `$${value.toLocaleString()}`;
          }
          return `$${Number(value).toLocaleString()}`;
        },
      },
      title: {
        display: true,
        text: subtitle,
        color: '#6b7280',
        font: {
          size: 12,
          weight: 500 as const,
        },
      },
    },
    x: {
      grid: {
        display: false,
      },
      ticks: {
        color: '#6b7280',
        font: {
          size: 11,
        },
      },
    },
  },
  animation: {
    duration: 500,
    easing: 'easeOutQuart',
  },
});
