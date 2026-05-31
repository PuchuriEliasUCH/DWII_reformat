---
name: AltaMesa
colors:
  surface: '#fff8f5'
  surface-dim: '#e6d7ce'
  surface-bright: '#fff8f5'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#fff1e9'
  surface-container: '#fbebe2'
  surface-container-high: '#f5e5dc'
  surface-container-highest: '#efe0d6'
  on-surface: '#221a15'
  on-surface-variant: '#55433b'
  inverse-surface: '#382f29'
  inverse-on-surface: '#feeee4'
  outline: '#88726a'
  outline-variant: '#dbc1b7'
  surface-tint: '#99461c'
  primary: '#964319'
  on-primary: '#ffffff'
  primary-container: '#b65b2f'
  on-primary-container: '#fffbff'
  inverse-primary: '#ffb596'
  secondary: '#546430'
  on-secondary: '#ffffff'
  secondary-container: '#d4e7a5'
  on-secondary-container: '#596833'
  tertiary: '#7a5500'
  on-tertiary: '#ffffff'
  tertiary-container: '#996c04'
  on-tertiary-container: '#fffbff'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#ffdbcd'
  primary-fixed-dim: '#ffb596'
  on-primary-fixed: '#360f00'
  on-primary-fixed-variant: '#7b2f04'
  secondary-fixed: '#d7eaa8'
  secondary-fixed-dim: '#bbce8e'
  on-secondary-fixed: '#151f00'
  on-secondary-fixed-variant: '#3d4c1a'
  tertiary-fixed: '#ffdeaa'
  tertiary-fixed-dim: '#f5bd58'
  on-tertiary-fixed: '#271900'
  on-tertiary-fixed-variant: '#5f4100'
  background: '#fff8f5'
  on-background: '#221a15'
  surface-variant: '#efe0d6'
typography:
  display-lg:
    fontFamily: Plus Jakarta Sans
    fontSize: 48px
    fontWeight: '700'
    lineHeight: 60px
    letterSpacing: -0.02em
  headline-lg:
    fontFamily: Plus Jakarta Sans
    fontSize: 32px
    fontWeight: '700'
    lineHeight: 40px
    letterSpacing: -0.01em
  headline-lg-mobile:
    fontFamily: Plus Jakarta Sans
    fontSize: 28px
    fontWeight: '700'
    lineHeight: 36px
  headline-md:
    fontFamily: Plus Jakarta Sans
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
  headline-sm:
    fontFamily: Plus Jakarta Sans
    fontSize: 20px
    fontWeight: '600'
    lineHeight: 28px
  body-lg:
    fontFamily: Plus Jakarta Sans
    fontSize: 18px
    fontWeight: '400'
    lineHeight: 28px
  body-md:
    fontFamily: Plus Jakarta Sans
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  body-sm:
    fontFamily: Plus Jakarta Sans
    fontSize: 14px
    fontWeight: '400'
    lineHeight: 20px
  label-lg:
    fontFamily: Plus Jakarta Sans
    fontSize: 14px
    fontWeight: '600'
    lineHeight: 20px
    letterSpacing: 0.01em
  label-sm:
    fontFamily: Plus Jakarta Sans
    fontSize: 12px
    fontWeight: '700'
    lineHeight: 16px
    letterSpacing: 0.04em
rounded:
  sm: 0.25rem
  DEFAULT: 0.5rem
  md: 0.75rem
  lg: 1rem
  xl: 1.5rem
  full: 9999px
spacing:
  base: 8px
  container-margin: 24px
  gutter: 16px
  stack-sm: 4px
  stack-md: 12px
  stack-lg: 24px
  section-gap: 40px
---

## Brand & Style

The design system is rooted in the "Modern Hospitality" aesthetic—a blend of high-end culinary warmth and professional SaaS efficiency. It is designed specifically for restaurant staff and managers who require a tool that feels as welcoming as a well-lit dining room but as disciplined as a Michelin-starred kitchen.

The visual style is **Corporate / Modern** with a **Tactile** touch. It avoids the coldness of typical enterprise software by utilizing a warm base palette and soft, organic shapes. The interface prioritizes clarity, organization, and a reduction of cognitive load, ensuring that in high-pressure service environments, the software remains an invisible, helpful partner rather than a digital obstacle.

**Core Principles:**
- **Intentional Warmth:** Every interaction should feel human and inviting.
- **Operational Clarity:** High-contrast typography and clear status indicators ensure speed of service.
- **Premium Reliability:** A stable, structured layout that evokes a sense of quality and trustworthiness.

## Colors

The palette is inspired by natural earth tones and high-quality raw ingredients. 

- **Primary (Terracotta):** Used for primary actions, active navigation states, and brand highlights.
- **Secondary (Olive Green):** Used for secondary actions, health indicators, and organic growth metrics.
- **Accent (Mustard):** Reserved for highlights, special notifications, and drawing attention to non-critical updates.
- **Typography:** The text colors utilize a warm charcoal base (`#3A312B`) instead of pure black to maintain the "Warm Ivory" atmosphere while ensuring WCAG AA legibility.
- **Functional States:** Status colors are slightly desaturated to harmonize with the warm background, preventing them from feeling "fluorescent" or jarring in a professional setting.

## Typography

This design system uses **Plus Jakarta Sans** for all roles. Its modern, slightly rounded apertures complement the hospitality theme while maintaining the geometric precision required for a dashboard.

- **Scale:** A tight scale is used for body text to maximize information density in table views and floor plans.
- **Hierarchy:** Use `label-sm` (uppercase) for table headers and small metadata categories.
- **Readability:** Line heights are generous in body styles to prevent eye strain during long shifts.

## Layout & Spacing

The layout utilizes a **Fixed Grid** philosophy for desktop dashboards to ensure the UI feels stable and "anchored," while transitioning to a **Fluid Grid** for tablet/POS views to maximize touch targets.

- **Desktop (1440px+):** 12-column grid, 24px margins, 16px gutters. Max content width of 1280px.
- **Tablet (768px - 1024px):** 8-column grid, 20px margins. This is the primary interface for floor staff.
- **Mobile (Below 768px):** 4-column grid, 16px margins. Used primarily for manager quick-checks.

**Spacing Rhythm:** All spacing must be multiples of the 8px base unit. Use `stack-md` for spacing between related form elements and `stack-lg` for spacing between distinct card sections.

## Elevation & Depth

Visual hierarchy is achieved through **Tonal Layers** combined with **Ambient Shadows**. This design system avoids harsh blacks in shadows, opting instead for a warm, tinted shadow that feels integrated with the ivory background.

- **Level 0 (Base):** Warm Ivory background (`#F8F4EE`).
- **Level 1 (Card):** White surface (`#FFFFFF`) with a 1px border in `#E6DFD5`. No shadow. Used for secondary information.
- **Level 2 (Active):** White surface with a "Soft Float" shadow (Offset: 0, 4px; Blur: 12px; Color: `rgba(58, 49, 43, 0.08)`). Used for primary cards and dashboard widgets.
- **Level 3 (Overlay):** Used for modals and dropdowns. Features a "Deep Float" shadow (Offset: 0, 12px; Blur: 24px; Color: `rgba(58, 49, 43, 0.12)`).

**Glass & Blurs:** Not used. Depth is purely a matter of layering and soft shadows to maintain a clean, SaaS-professional look.

## Shapes

The shape language is consistently **Rounded**, reflecting the approachable nature of hospitality.

- **Standard Elements (Buttons, Inputs, Small Cards):** 0.5rem (8px) corner radius.
- **Large Containers (Dashboard Widgets, Modals):** 1rem (16px) corner radius.
- **Search Bars & Tags:** Pill-shaped (fully rounded) to distinguish them from actionable buttons.

Borders are strictly 1px wide, utilizing the `#E6DFD5` color to provide structure without adding visual noise.

## Components

### Buttons
- **Primary:** Terracotta background, White text. High-contrast, 8px radius.
- **Secondary:** Olive Green background, White text. Used for "Add" or "Confirm" actions.
- **Ghost:** No background, Terracotta text. Used for "Cancel" or "Back" actions.

### Input Fields
- Inputs use the `Surface Background` (`#FFFDF8`) to differentiate from the `Card Background`. 
- 1px border in `#E6DFD5`. Focus state uses a 2px Terracotta border.

### Status Chips
- Small, uppercase text.
- Lightly tinted background with a high-contrast text color (e.g., "Ready" uses a light green background with Dark Green text).

### Floor Plan Tables
- **Available:** White with Olive Green dashed border.
- **Occupied:** Solid Mustard with White text.
- **Selected:** Terracotta border (2px).

### Cards
- Always use the `Card Background` (`#FFFFFF`).
- Headers within cards should use a 1px bottom border in `#E6DFD5` to separate title from content.

### Navigation
- Sidebar-based for desktop. Use a subtle vertical divider. Active items use a Terracotta "indicator" bar on the left edge and a light Ivory hover state.