using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Presentation.UI.Screens
{
    public class BootstrapScreen : Screen<BootstrapScreen.Parameters, BootstrapScreen.Result>
    {
        [SerializeField] private TMPro.TMP_Text _text;
        public class Parameters
        {
            public int i = 0;
        }

        public class Result
        {
            public bool success = false;
        }

        public override async UniTask Show(Parameters parameters, bool immediately,
        CancellationToken ct, Action<Result> callback = null)
        {
            await base.Show(parameters, immediately, ct, callback);
            _result = new Result() { success = true };
            _text.text = parameters.i.ToString();
        }
    }
}