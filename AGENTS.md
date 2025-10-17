# Blazor Component Refactor Prompt Template

## Project Overview
**Goal**: Extract section UIs from a monolithic Blazor layout component into shared, reusable components with proper parameter binding and two-way communication.

## Core Requirements

### 1. Component Structure
- **Namespace**: `QuizManager.Shared`
- **Sections**: Student, Company, Professor, ResearchGroup
- **Naming Convention**: `<Section>LayoutSection` (e.g., `StudentLayoutSection`)
- **Architecture**: Shared components with code-behind separation

### 2. Technical Constraints
- **Target**: Blazor Server or WASM, .NET 7+ with nullable enabled
- **No @page directive**: Do not add `@page` to shared components
- **Partial classes**: Use `public partial class ComponentName` in `.razor.cs` files
- **No ComponentBase inheritance**: In `.razor.cs` files paired with `.razor`, do NOT inherit from `ComponentBase`
- **CSS isolation**: Use `.razor.css` files beside each component, no inline styles
- **PascalCase parameters**: Use PascalCase for all component parameters

### 3. Parameter Binding Pattern
Each section component must implement this exact pattern:

```csharp
// In [Section]LayoutSection.razor.cs
public partial class [Section]LayoutSection
{
    [Parameter] public bool IsInitialized { get; set; }
    [Parameter] public bool IsRegistered { get; set; }
    [Parameter] public EventCallback<bool> IsRegisteredChanged { get; set; }

    protected async Task SetRegistered(bool value)
    {
        IsRegistered = value;
        if (IsRegisteredChanged.HasDelegate)
            await IsRegisteredChanged.InvokeAsync(value);
    }
}
```

### 4. Parent Component Integration
The parent component (MainLayout.razor.cs) must include:

```csharp
// Private fields for state management
private bool isStudentInitialized, isCompanyInitialized, isProfessorInitialized, isResearchGroupInitialized;
private bool isStudentRegistered, isCompanyRegistered, isProfessorRegistered, isResearchGroupRegistered;

// EventCallback handlers for two-way binding
private async Task OnStudentRegisteredChanged(bool value)
{
    isStudentRegistered = value;
    await InvokeAsync(StateHasChanged);
}
// Repeat for other sections...
```

### 5. Parent Usage Pattern
In MainLayout.razor:

```razor
@if (UserRole == "Student")
{
    <StudentLayoutSection @key="UserRole"
                         IsInitialized="isStudentInitialized"
                         IsRegistered="isStudentRegistered"
                         IsRegisteredChanged="OnStudentRegisteredChanged" />
}
<!-- Repeat for other sections -->
```

## Implementation Strategy

### Phase 1: Baseline Creation
1. **Create working baseline branch** from a known good commit
2. **Fix immediate compilation issues** (duplicate fields, missing references)
3. **Establish clean starting point** with minimal errors

### Phase 2: Component Refactor
1. **Add parameters to all section components** using the exact pattern above
2. **Update parent component** with required fields and EventCallback handlers
3. **Update parent usage** to pass parameters correctly
4. **Test with minimal implementations** to verify structure works

### Phase 3: Full Implementation
1. **Gradually restore functionality** from original implementations
2. **Fix missing variables and references** as they appear
3. **Maintain parameter binding** throughout the process

### Phase 4: CSS Isolation (Optional)
1. **Extract inline styles** to component-specific `.razor.css` files
2. **Categorize styles** by component responsibility
3. **Remove inline `<style>` blocks** from parent component

## Common Issues & Solutions

### Issue 1: Configuration Errors
**Problem**: `CS0103: The name 'Configuration' does not exist in the current context`
**Solution**: Replace `new AppDbContext(Configuration)` with injected `dbContext` instance

### Issue 2: Duplicate Field Definitions
**Problem**: `CS0102: The type already contains a definition for 'fieldName'`
**Solution**: Remove duplicate field declarations, keep only one instance

### Issue 3: Missing Variables in Child Components
**Problem**: Child components reference variables that don't exist
**Solution**: Either add missing variables or update references to use parameters

### Issue 4: Parameter Binding Warnings
**Problem**: Linter warnings about missing required parameters
**Solution**: Use explicit parameter passing instead of `@bind-` syntax for complex scenarios

## Testing Strategy

### 1. Minimal Test Components
Create simple test versions of each section component:

```razor
@if (!IsInitialized)
{
    <div class="alert alert-info">
        <h4>[Section] Section - Loading</h4>
        <p>IsInitialized: @IsInitialized</p>
        <p>IsRegistered: @IsRegistered</p>
    </div>
}
else if (!IsRegistered)
{
    <div class="alert alert-warning">
        <h4>[Section] Section - Not Registered</h4>
        <button class="btn btn-primary" @onclick="() => SetRegistered(true)">Register</button>
    </div>
}
else
{
    <div class="alert alert-success">
        <h4>[Section] Section - Registered</h4>
        <button class="btn btn-secondary" @onclick="() => SetRegistered(false)">Unregister</button>
    </div>
}
```

### 2. Build Verification
- **Target**: 0 compilation errors
- **Method**: `dotnet build --nologo --verbosity minimal`
- **Success Criteria**: Clean build with only warnings (no errors)

### 3. Application Startup Test
- **Method**: `dotnet run --no-build` with timeout
- **Success Criteria**: Application starts without crashing

## File Structure
```
Shared/
├── MainLayout.razor
├── MainLayout.razor.cs
├── MainLayout.razor.css
├── StudentLayoutSection.razor
├── StudentLayoutSection.razor.cs
├── StudentLayoutSection.razor.css
├── CompanyLayoutSection.razor
├── CompanyLayoutSection.razor.cs
├── CompanyLayoutSection.razor.css
├── ProfessorLayoutSection.razor
├── ProfessorLayoutSection.razor.cs
├── ProfessorLayoutSection.razor.css
├── ResearchGroupLayoutSection.razor
├── ResearchGroupLayoutSection.razor.cs
└── ResearchGroupLayoutSection.razor.css
```

## Success Metrics
- ✅ **0 compilation errors**
- ✅ **Application starts successfully**
- ✅ **Parameter binding works correctly**
- ✅ **Two-way communication functions**
- ✅ **Clean component separation**
- ✅ **Maintainable code structure**

## Key Learnings
1. **Start with a working baseline** - Don't try to fix everything at once
2. **Use minimal test components** - Verify structure before adding complexity
3. **Fix compilation errors incrementally** - Address one issue type at a time
4. **Test frequently** - Build after each major change
5. **Maintain parameter consistency** - Use the exact pattern for all components
6. **Preserve original functionality** - Keep backups of working implementations

## Example Usage
```bash
# 1. Create baseline branch
git checkout -b working-baseline <known-good-commit>

# 2. Fix immediate issues
# Remove duplicate fields, fix Configuration references

# 3. Add parameters to components
# Update all [Section]LayoutSection.razor.cs files

# 4. Update parent component
# Add fields and EventCallback handlers to MainLayout.razor.cs

# 5. Update parent usage
# Modify MainLayout.razor to pass parameters

# 6. Test with minimal components
# Replace complex implementations with simple test versions

# 7. Verify success
dotnet build  # Should show 0 errors
dotnet run    # Should start successfully
```

This template provides a complete roadmap for successfully refactoring Blazor components with proper parameter binding and two-way communication.
