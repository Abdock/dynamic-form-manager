<!-- src/components/CustomDatePicker.vue -->
<script setup>
import { ref, computed, watch, onMounted, onUnmounted } from 'vue';
import dayjs from 'dayjs';
import { Calendar as CalendarIcon, ChevronLeft, ChevronRight, X } from 'lucide-vue-next';

const props = defineProps({
    modelValue: {
        type: String,
        default: () => dayjs().format('YYYY-MM-DD')
    },
    label: {
        type: String,
        default: 'Date'
    },
    placeholder: {
        type: String,
        default: 'Select date'
    },
    minDate: {
        type: String,
        default: null
    },
    maxDate: {
        type: String,
        default: null
    },
    required: {
        type: Boolean,
        default: false
    },
    id: {
        type: String,
        default: () => `date-picker-${Math.random().toString(36).substring(2, 9)}`
    }
});

const emit = defineEmits(['update:modelValue']);

const isOpen = ref(false);
const currentMonth = ref(dayjs(props.modelValue || undefined));
const selectedDate = ref(props.modelValue ? dayjs(props.modelValue) : null);
const inputRef = ref(null);
const calendarRef = ref(null);

const formattedSelectedDate = computed(() => {
    if (!selectedDate.value) return '';
    return selectedDate.value.format('MMMM D, YYYY');
});

const daysInMonth = computed(() => {
    const firstDayOfMonth = currentMonth.value.startOf('month');
    const daysInMonth = currentMonth.value.daysInMonth();
    const dayOfWeek = firstDayOfMonth.day();

    const days = [];

    for (let i = 0; i < dayOfWeek; i++) {
        days.push({
            date: firstDayOfMonth.subtract(dayOfWeek - i, 'day'),
            isCurrentMonth: false,
            isSelected: false,
            isToday: false,
            isDisabled: true
        });
    }

    const today = dayjs();
    for (let i = 1; i <= daysInMonth; i++) {
        const date = currentMonth.value.set('date', i);
        const isDisabled = (props.minDate && date.isBefore(dayjs(props.minDate))) ||
            (props.maxDate && date.isAfter(dayjs(props.maxDate)));

        days.push({
            date,
            isCurrentMonth: true,
            isSelected: selectedDate.value && date.isSame(selectedDate.value, 'day'),
            isToday: date.isSame(today, 'day'),
            isDisabled
        });
    }

    const remainingDays = 42 - days.length;
    if (remainingDays > 0 && remainingDays < 14) {
        for (let i = 1; i <= remainingDays; i++) {
            days.push({
                date: currentMonth.value.endOf('month').add(i, 'day'),
                isCurrentMonth: false,
                isSelected: false,
                isToday: false,
                isDisabled: true
            });
        }
    }

    return days;
});

const weekDays = computed(() => {
    return ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
});

const currentMonthName = computed(() => {
    return currentMonth.value.format('MMMM YYYY');
});

const toggleCalendar = () => {
    isOpen.value = !isOpen.value;
};

const closeCalendar = () => {
    isOpen.value = false;
};

const previousMonth = () => {
    currentMonth.value = currentMonth.value.subtract(1, 'month');
};

const nextMonth = () => {
    currentMonth.value = currentMonth.value.add(1, 'month');
};

const selectDate = (day) => {
    if (day.isDisabled) return;

    selectedDate.value = day.date;
    emit('update:modelValue', day.date.format('YYYY-MM-DD'));
    closeCalendar();
};

const clearDate = (event) => {
    event.stopPropagation();
    selectedDate.value = null;
    emit('update:modelValue', '');
};

const handleClickOutside = (event) => {
    if (isOpen.value && calendarRef.value && !calendarRef.value.contains(event.target) && !inputRef.value.contains(event.target)) {
        closeCalendar();
    }
};

watch(() => props.modelValue, (newValue) => {
    if (newValue) {
        selectedDate.value = dayjs(newValue);
        currentMonth.value = dayjs(newValue);
    } else {
        selectedDate.value = null;
    }
});

onMounted(() => {
    document.addEventListener('click', handleClickOutside);
});

onUnmounted(() => {
    document.removeEventListener('click', handleClickOutside);
});
</script>

<template>
    <div class="custom-datepicker">
        <label :for="id" class="datepicker-label">
            {{ label }}
            <span v-if="required" class="required-mark">*</span>
        </label>

        <div class="datepicker-input-wrapper" ref="inputRef">
            <div
                :id="id"
                class="datepicker-input"
                @click="toggleCalendar"
                :class="{ 'has-value': selectedDate }"
            >
                <CalendarIcon class="calendar-icon" size="18" />
                <span v-if="selectedDate" class="selected-date">{{ formattedSelectedDate }}</span>
                <span v-else class="placeholder">{{ placeholder }}</span>

                <button
                    v-if="selectedDate"
                    type="button"
                    class="clear-button"
                    @click="clearDate"
                    aria-label="Clear date"
                >
                    <X size="16" />
                </button>
            </div>

            <!-- Calendar dropdown -->
            <div v-if="isOpen" class="calendar-dropdown" ref="calendarRef">
                <div class="calendar-header">
                    <button type="button" class="month-nav" @click="previousMonth">
                        <ChevronLeft size="20" />
                    </button>
                    <div class="current-month">{{ currentMonthName }}</div>
                    <button type="button" class="month-nav" @click="nextMonth">
                        <ChevronRight size="20" />
                    </button>
                </div>

                <div class="weekdays">
                    <div v-for="day in weekDays" :key="day" class="weekday">{{ day }}</div>
                </div>

                <div class="calendar-grid">
                    <div
                        v-for="(day, index) in daysInMonth"
                        :key="index"
                        class="calendar-day"
                        :class="{
              'other-month': !day.isCurrentMonth,
              'is-selected': day.isSelected,
              'is-today': day.isToday,
              'is-disabled': day.isDisabled
            }"
                        @click="selectDate(day)"
                    >
                        {{ day.date.date() }}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.custom-datepicker {
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
    position: relative;
}

.datepicker-label {
    font-weight: 500;
    color: #555;
    font-size: 0.95rem;
}

.required-mark {
    color: #e44;
    margin-left: 4px;
}

.datepicker-input-wrapper {
    position: relative;
}

.datepicker-input {
    display: flex;
    align-items: center;
    padding: 0.75rem;
    border-radius: 4px;
    border: 1px solid #ccc;
    background-color: white;
    cursor: pointer;
    min-height: 42px;
    transition: border-color 0.2s, box-shadow 0.2s;
    gap: 0.5rem;
}

.datepicker-input:hover {
    border-color: #aaa;
}

.datepicker-input.has-value {
    border-color: #646cff;
}

.calendar-icon {
    color: #646cff;
    flex-shrink: 0;
}

.placeholder {
    color: #999;
}

.selected-date {
    flex-grow: 1;
    color: #333;
}

.clear-button {
    background: transparent;
    border: none;
    padding: 0;
    cursor: pointer;
    color: #999;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: color 0.2s;
}

.clear-button:hover {
    color: #e44;
}

.calendar-dropdown {
    position: absolute;
    top: calc(100% + 5px);
    left: 0;
    z-index: 10;
    width: 320px;
    background: white;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    padding: 1rem;
    animation: fadeIn 0.2s ease-out;
}

@keyframes fadeIn {
    from { opacity: 0; transform: translateY(-10px); }
    to { opacity: 1; transform: translateY(0); }
}

.calendar-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1rem;
}

.month-nav {
    background: transparent;
    border: none;
    color: #646cff;
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 5px;
    border-radius: 4px;
    transition: background-color 0.2s;
    outline: none;
}

.month-nav:hover {
    background-color: #f0f0fe;
}

.current-month {
    font-weight: 600;
    color: #333;
}

.weekdays {
    display: grid;
    grid-template-columns: repeat(7, 1fr);
    gap: 5px;
    margin-bottom: 0.5rem;
}

.weekday {
    text-align: center;
    font-size: 0.85rem;
    font-weight: 600;
    color: #666;
    padding: 5px 0;
}

.calendar-grid {
    display: grid;
    grid-template-columns: repeat(7, 1fr);
    grid-gap: 5px;
}

.calendar-day {
    display: flex;
    align-items: center;
    justify-content: center;
    height: 36px;
    border-radius: 50%;
    cursor: pointer;
    transition: all 0.2s;
    font-size: 0.95rem;
}

.calendar-day:hover:not(.other-month):not(.is-disabled) {
    background-color: #f0f0fe;
}

.other-month {
    color: #ccc;
}

.is-selected {
    background-color: #646cff;
    color: white;
    font-weight: 600;
}

.is-today:not(.is-selected) {
    border: 1px solid #646cff;
    color: #646cff;
    font-weight: 600;
}

.is-disabled {
    opacity: 0.5;
    cursor: not-allowed;
}

@media (max-width: 480px) {
    .calendar-dropdown {
        width: 100%;
        left: 0;
        right: 0;
    }
}
</style>