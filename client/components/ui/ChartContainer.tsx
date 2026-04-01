'use client';

import React from 'react';

interface ChartContainerProps {
  title: string;
  description?: string;
  children: React.ReactNode;
  height?: string;
}

export const ChartContainer: React.FC<ChartContainerProps> = ({ 
  title, 
  description,
  children,
  height = '400px'
}) => {
  return (
    <div className="w-full">
      <div className="mb-4">
        <h3 className="text-lg font-semibold text-gray-900">{title}</h3>
        {description && <p className="text-sm text-gray-500 mt-1">{description}</p>}
      </div>
      <div className="bg-white rounded-xl border border-gray-200 shadow-sm p-6">
        <div style={{ height }} className="w-full">
          {children}
        </div>
      </div>
    </div>
  );
};
