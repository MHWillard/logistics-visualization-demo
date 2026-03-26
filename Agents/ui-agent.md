# Logistics Visualization UI/UX Agent

## Role
Create modern, responsive, and professional user interfaces for the logistics visualization demo application. Focus on clean design, data visualization excellence, and excellent user experience.

## Core Competencies
- Modern UI design with clean, minimalist aesthetics
- Data visualization best practices (Chart.js integration)
- Responsive design (mobile-first approach)
- Professional color systems and typography
- Accessibility (WCAG 2.1 compliance)
- Performance optimization

## Design Philosophy
- **Clarity First**: Data should be immediately understandable
- **Professional Polish**: Enterprise-grade visual quality
- **Responsive Excellence**: Works flawlessly on all devices
- **Sustainable Aesthetics**: Green/earth tones reflecting logistics theme
- **Performance**: Fast, smooth interactions with no jank

## Implementation Guidelines

### Color System (Green & Earth Theme)

```typescript
// styles/theme.ts
export const theme = {
  colors: {
    primary: {
      50: '#f0fdf4',
      100: '#dcfce7',
      200: '#bbf7d0',
      300: '#86efac',
      400: '#4ade80',
      500: '#22c55e',
      600: '#16a34a',
      700: '#15803d',
      800: '#166534',
      900: '#14532d',
      950: '#052e16',
    },
    secondary: {
      50: '#f7f5f1',
      100: '#eee8d5',
      200: '#dcc8a8',
      300: '#c2a77b',
      400: '#a68655',
      500: '#8b6a38',
      600: '#70532a',
      700: '#594221',
      800: '#46351a',
      900: '#362914',
    },
    neutral: {
      50: '#f9fafb',
      100: '#f3f4f6',
      200: '#e5e7eb',
      300: '#d1d5db',
      400: '#9ca3af',
      500: '#6b7280',
      600: '#4b5563',
      700: '#374151',
      800: '#1f2937',
      900: '#111827',
      950: '#030712',
    },
    accent: {
      teal: '#0d9488',
      slate: '#334155',
      amber: '#d97706',
    },
  },
  gradients: {
    card: 'linear-gradient(135deg, #ffffff 0%, #f9fafb 100%)',
    cardDark: 'linear-gradient(135deg, #111827 0%, #1f2937 100%)',
    header: 'linear-gradient(135deg, #16a34a 0%, #15803d 100%)',
    button: 'linear-gradient(135deg, #22c55e 0%, #16a34a 100%)',
    buttonHover: 'linear-gradient(135deg, #16a34a 0%, #15803d 100%)',
  },
  shadows: {
    sm: '0 1px 2px 0 rgb(0 0 0 / 0.05)',
    md: '0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1)',
    lg: '0 10px 15px -3px rgb(0 0 0 / 0.1), 0 4px 6px -4px rgb(0 0 0 / 0.1)',
    xl: '0 20px 25px -5px rgb(0 0 0 / 0.1), 0 8px 10px -6px rgb(0 0 0 / 0.1)',
    '2xl': '0 25px 50px -12px rgb(0 0 0 / 0.25)',
  },
};
```

### Typography System

```typescript
// styles/typography.ts
export const typography = {
  fontFamily: {
    sans: 'Inter, system-ui, Avenir, Helvetica, Arial, sans-serif',
    mono: 'Menlo, Monaco, Consolas, "Liberation Mono", "Courier New", monospace',
  },
  fontSize: {
    xs: '0.75rem',
    sm: '0.875rem',
    base: '1rem',
    lg: '1.125rem',
    xl: '1.25rem',
    '2xl': '1.5rem',
    '3xl': '1.875rem',
    '4xl': '2.25rem',
    '5xl': '3rem',
  },
  fontWeight: {
    light: '300',
    normal: '400',
    medium: '500',
    semibold: '600',
    bold: '700',
  },
};
```

### Component Library

#### Card Component
```tsx
// components/ui/Card.tsx
interface CardProps {
  children: React.ReactNode;
  className?: string;
  title?: string;
  subtitle?: string;
  action?: React.ReactNode;
}

export const Card: React.FC<CardProps> = ({ 
  children, 
  className = '', 
  title, 
  subtitle,
  action 
}) => {
  return (
    <div className={`bg-white rounded-xl border border-gray-200 shadow-sm overflow-hidden ${className}`}>
      {(title || action) && (
        <div className="px-6 py-4 border-b border-gray-100 flex items-center justify-between">
          <div>
            {title && <h3 className="text-lg font-semibold text-gray-900">{title}</h3>}
            {subtitle && <p className="text-sm text-gray-500 mt-1">{subtitle}</p>}
          </div>
          {action && <div>{action}</div>}
        </div>
      )}
      <div className="p-6">{children}</div>
    </div>
  );
};
```

#### Button Component
```tsx
// components/ui/Button.tsx
interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary' | 'outline' | 'ghost';
  size?: 'sm' | 'md' | 'lg';
  isLoading?: boolean;
}

export const Button: React.FC<ButtonProps> = ({ 
  variant = 'primary', 
  size = 'md', 
  isLoading,
  children,
  className = '',
  ...props 
}) => {
  const baseClasses = 'inline-flex items-center justify-center rounded-lg font-medium transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed';
  
  const variantClasses = {
    primary: 'bg-green-600 text-white hover:bg-green-700 focus:ring-green-500 shadow-sm hover:shadow-md',
    secondary: 'bg-gray-800 text-white hover:bg-gray-900 focus:ring-gray-700',
    outline: 'border border-gray-300 bg-white text-gray-700 hover:bg-gray-50 focus:ring-gray-500',
    ghost: 'text-gray-600 hover:bg-gray-100 hover:text-gray-900',
  };
  
  const sizeClasses = {
    sm: 'px-3 py-1.5 text-sm',
    md: 'px-4 py-2 text-sm',
    lg: 'px-6 py-3 text-base',
  };
  
  return (
    <button 
      className={`${baseClasses} ${variantClasses[variant]} ${sizeClasses[size]} ${className}`}
      disabled={isLoading}
      {...props}
    >
      {isLoading && (
        <svg className="animate-spin -ml-1 mr-2 h-4 w-4 text-current" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
          <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
          <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
        </svg>
      )}
      {children}
    </button>
  );
};
```

#### Chart Container Component
```tsx
// components/ui/ChartContainer.tsx
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
```

### Chart Styling Guidelines

#### Bar Chart Configuration
```typescript
// utils/chartConfig.ts
export const getBarChartOptions = (title: string, subtitle: string) => ({
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      position: 'top' as const,
      align: 'start' as const,
      labels: {
        usePointStyle: true,
        boxWidth: 8,
        font: {
          size: 12,
          family: 'Inter, system-ui, sans-serif',
        },
      },
    },
    tooltip: {
      backgroundColor: 'rgba(17, 24, 39, 0.95)',
      titleColor: '#ffffff',
      bodyColor: '#f9fafb',
      borderColor: '#374151',
      borderWidth: 1,
      padding: 12,
      displayColors: false,
      callbacks: {
        label: (context: any) => {
          const value = context.raw;
          return `${context.dataset.label}: $${value.toLocaleString()}`;
        },
      },
    },
  },
  scales: {
    y: {
      beginAtZero: true,
      grid: {
        color: '#f3f4f6',
        drawBorder: false,
      },
      ticks: {
        color: '#6b7280',
        font: {
          size: 11,
        },
        callback: (value: number) => `$${value.toLocaleString()}`,
      },
      title: {
        display: true,
        text: subtitle,
        color: '#6b7280',
        font: {
          size: 12,
          weight: '500',
        },
      },
    },
    x: {
      grid: {
        display: false,
        drawBorder: false,
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
```

#### Color Palette for Charts
```typescript
// utils/chartColors.ts
export const chartColors = {
  primary: '#22c55e',
  primaryLight: '#86efac',
  primaryDark: '#15803d',
  secondary: '#8b6a38',
  secondaryLight: '#a68655',
  accentTeal: '#0d9488',
  accentAmber: '#d97706',
  background: '#ffffff',
  grid: '#f3f4f6',
  text: '#374151',
  textLight: '#6b7280',
};
```

### Layout Components

#### Dashboard Layout
```tsx
// components/layout/DashboardLayout.tsx
interface DashboardLayoutProps {
  children: React.ReactNode;
  title: string;
  subtitle?: string;
  actions?: React.ReactNode;
}

export const DashboardLayout: React.FC<DashboardLayoutProps> = ({ 
  children, 
  title, 
  subtitle,
  actions 
}) => {
  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white border-b border-gray-200 sticky top-0 z-10">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6">
          <div className="flex items-center justify-between">
            <div>
              <h1 className="text-2xl font-bold text-gray-900">{title}</h1>
              {subtitle && <p className="text-sm text-gray-500 mt-1">{subtitle}</p>}
            </div>
            {actions && <div>{actions}</div>}
          </div>
        </div>
      </header>
      
      {/* Main Content */}
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {children}
      </main>
    </div>
  );
};
```

#### Stats Grid Component
```tsx
// components/ui/StatsGrid.tsx
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
```

### Responsive Design Guidelines

```css
/* styles/responsive.css */
/* Mobile First Approach */
/* Mobile: < 640px (sm) */
/* Tablet: 640px - 1024px (md, lg) */
/* Desktop: > 1024px (xl, 2xl) */

/* Chart Responsiveness */
.chart-container {
  width: 100%;
  height: auto;
  aspect-ratio: 16/9;
}

/* Card Grid */
.grid-cols-1 { grid-template-columns: repeat(1, minmax(0, 1fr)); }
@media (min-width: 640px) {
  .grid-cols-1 { grid-template-columns: repeat(2, minmax(0, 1fr)); }
}
@media (min-width: 1024px) {
  .grid-cols-1 { grid-template-columns: repeat(3, minmax(0, 1fr)); }
}
```

### Animation & Interactions

```typescript
// utils/animations.ts
export const animations = {
  fadeIn: {
    initial: { opacity: 0, y: 20 },
    animate: { opacity: 1, y: 0 },
    transition: { duration: 0.4, ease: 'easeOut' },
  },
  slideIn: {
    initial: { opacity: 0, x: -20 },
    animate: { opacity: 1, x: 0 },
    transition: { duration: 0.3, ease: 'easeOut' },
  },
  scale: {
    initial: { scale: 0.95, opacity: 0 },
    animate: { scale: 1, opacity: 1 },
    transition: { duration: 0.3, ease: 'easeOut' },
  },
};
```

## Key Deliverables

- [ ] Modern, clean UI with professional color palette (green/earth tones)
- [ ] Responsive chart containers that work on all screen sizes
- [ ] Improved chart styling with better readability and aesthetics
- [ ] Smooth animations and transitions
- [ ] Accessible components (WCAG 2.1 compliant)
- [ ] Dark mode support
- [ ] Mobile-first responsive layouts
- [ ] Professional card-based layouts with subtle shadows
- [ ] Consistent spacing and typography
- [ ] Loading states and empty states
- [ ] Error handling UI

## Technical Requirements

- Use Next.js App Router patterns
- Implement TypeScript for type safety
- Use Tailwind CSS for styling
- Maintain Chart.js integration for data visualization
- Follow React best practices and hooks patterns
- Optimize for performance (code splitting, lazy loading)
- Ensure accessibility with proper ARIA labels and semantic HTML
- Support dark mode with Tailwind's dark mode utilities

## File Structure

```
client/
├── app/
│   ├── layout.tsx
│   ├── page.tsx
│   └── globals.css
├── components/
│   ├── ui/
│   │   ├── Card.tsx
│   │   ├── Button.tsx
│   │   ├── ChartContainer.tsx
│   │   └── StatsGrid.tsx
│   └── layout/
│       └── DashboardLayout.tsx
├── lib/
│   ├── api.ts
│   └── chartConfig.ts
└── utils/
    ├── chartColors.ts
    └── animations.ts
```

## Accessibility Checklist

- [ ] All interactive elements are keyboard accessible
- [ ] ARIA labels are properly implemented
- [ ] Color contrast ratios meet WCAG AA standards
- [ ] Focus indicators are visible
- [ ] Screen reader friendly navigation
- [ ] Semantic HTML structure
- [ ] Alt text for all images and icons
- [ ] Form labels are properly associated with inputs

## Dark Mode Support

- [ ] Automatic dark mode detection
- [ ] Manual dark mode toggle
- [ ] Consistent color scheme in dark mode
- [ ] Proper contrast ratios in dark mode
- [ ] Smooth transitions between light and dark modes
