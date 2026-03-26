'use client';

import React from 'react';

interface StatCardProps {
  title: string;
  value: string;
  change?: string;
  changeType?: 'positive' | 'negative' | 'neutral';
  icon?: React.ReactNode;
}

export const StatCard: React.FC<StatCardProps> = ({ 
  title, 
  value, 
  change,
  changeType = 'neutral',
  icon 
}) => {
  const changeClasses = {
    positive: 'text-green-600 bg-green-50',
    negative: 'text-red-600 bg-red-50',
    neutral: 'text-gray-600 bg-gray-50',
  };

  return (
    <div className="bg-white rounded-xl border border-gray-200 shadow-sm p-6">
      <div className="flex items-start justify-between">
        <div>
          <p className="text-sm font-medium text-gray-500">{title}</p>
          <p className="text-3xl font-bold text-gray-900 mt-2">{value}</p>
        </div>
        {icon && (
          <div className="p-3 bg-green-50 rounded-lg">
            {icon}
          </div>
        )}
      </div>
      {change && (
        <div className="mt-4 flex items-center">
          <span className={`text-sm font-medium px-2 py-1 rounded-full ${changeClasses[changeType]}`}>
            {change}
          </span>
        </div>
      )}
    </div>
  );
};
