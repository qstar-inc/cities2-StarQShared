using System;
using System.Collections.Generic;
using Colossal.UI.Binding;
using Game.UI.InGame;

namespace StarQ.Shared.Extensions.UI
{
    public abstract partial class ExtendedInfoSectionBase : InfoSectionBase
    {
        private readonly List<Action> _updateCallbacks = new();

        protected override void OnUpdate()
        {
            foreach (var action in _updateCallbacks)
            {
                action();
            }

            base.OnUpdate();
        }

        public ValueBindingHelper<T> CreateBinding<T>(string key, T initialValue)
        {
            var helper = new ValueBindingHelper<T>(
                new(StarQHelper.Id, key, initialValue, new GenericUIWriter<T?>())
            );

            AddBinding(helper.Binding);

            _updateCallbacks.Add(helper.ForceUpdate);

            return helper;
        }

        public ValueBindingHelper<T> CreateBinding<T>(
            string key,
            string setterKey,
            T initialValue,
            Action<T>? updateCallBack = null
        )
        {
            var helper = new ValueBindingHelper<T>(
                new(StarQHelper.Id, key, initialValue, new GenericUIWriter<T?>()),
                updateCallBack
            );
            var trigger = new TriggerBinding<T>(
                StarQHelper.Id,
                setterKey,
                helper.UpdateCallback,
                GenericUIReader<T>.Create()
            );

            AddBinding(helper.Binding);
            AddBinding(trigger);

            _updateCallbacks.Add(helper.ForceUpdate);

            return helper;
        }

        public GetterValueBinding<T> CreateBinding<T>(string key, Func<T> getterFunc)
        {
            var binding = new GetterValueBinding<T>(
                StarQHelper.Id,
                key,
                getterFunc,
                new GenericUIWriter<T>()
            );

            AddUpdateBinding(binding);

            return binding;
        }

        public TriggerBinding CreateTrigger(string key, Action action)
        {
            var binding = new TriggerBinding(StarQHelper.Id, key, action);

            AddBinding(binding);

            return binding;
        }

        public TriggerBinding<T1> CreateTrigger<T1>(string key, Action<T1> action)
        {
            var binding = new TriggerBinding<T1>(
                StarQHelper.Id,
                key,
                action,
                GenericUIReader<T1>.Create()
            );

            AddBinding(binding);

            return binding;
        }

        public TriggerBinding<T1, T2> CreateTrigger<T1, T2>(string key, Action<T1, T2> action)
        {
            var binding = new TriggerBinding<T1, T2>(
                StarQHelper.Id,
                key,
                action,
                GenericUIReader<T1>.Create(),
                GenericUIReader<T2>.Create()
            );

            AddBinding(binding);

            return binding;
        }

        public TriggerBinding<T1, T2, T3> CreateTrigger<T1, T2, T3>(
            string key,
            Action<T1, T2, T3> action
        )
        {
            var binding = new TriggerBinding<T1, T2, T3>(
                StarQHelper.Id,
                key,
                action,
                GenericUIReader<T1>.Create(),
                GenericUIReader<T2>.Create(),
                GenericUIReader<T3>.Create()
            );

            AddBinding(binding);

            return binding;
        }

        public TriggerBinding<T1, T2, T3, T4> CreateTrigger<T1, T2, T3, T4>(
            string key,
            Action<T1, T2, T3, T4> action
        )
        {
            var binding = new TriggerBinding<T1, T2, T3, T4>(
                StarQHelper.Id,
                key,
                action,
                GenericUIReader<T1>.Create(),
                GenericUIReader<T2>.Create(),
                GenericUIReader<T3>.Create(),
                GenericUIReader<T4>.Create()
            );

            AddBinding(binding);

            return binding;
        }
    }
}
