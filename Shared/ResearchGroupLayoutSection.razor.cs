using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuizManager.Data;
using QuizManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizManager.Shared
{
    public partial class ResearchGroupLayoutSection
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
}
