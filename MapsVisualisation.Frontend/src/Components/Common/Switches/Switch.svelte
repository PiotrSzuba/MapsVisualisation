<script lang="ts">
    export let isChecked: boolean;
    export let onChange: (event: Event & {currentTarget: EventTarget & HTMLInputElement}) => void;
</script>

<div class={$$props.class}>
    <label class="switch">
        <input type="checkbox" bind:checked={isChecked} on:change={onChange}>
        <span class="slider round"></span>
    </label>
</div>

<style>
.switch {
  position: relative;
  display: inline-block;
  width: 40px;
  height: 20px;
  bottom: -4px;
}

.switch input {
  opacity: 0;
  width: 0;
  height: 0;
}

.slider {
  position: absolute;
  cursor: pointer;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgb(25, 194, 206);
  -webkit-transition: .4s;
  transition: .4s;
}

.slider:before {
  position: absolute;
  content: "";
  height: 20px;
  width: 20px;
  left: 0px;
  bottom: 0px;
  background-color: white;
  -webkit-transition: .2s;
  transition: .2s;
}

input:checked + .slider {
  background-color: #16cf63;
}

input:focus + .slider {
  box-shadow: 0 0 1px #16cf63;
}

input:checked + .slider:before {
  -webkit-transform: translateX(20px);
  -ms-transform: translateX(20px);
  transform: translateX(20px);
}

.slider.round {
  border-radius: 30px;
}

.slider.round:before {
  border-radius: 50%;
}
</style>