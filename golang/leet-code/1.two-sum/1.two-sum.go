package main

import "fmt"

func main() {
	fmt.Println("starting")

	fmt.Println(twoSum([]int{2, 7, 11, 15}, 9))
	fmt.Println(twoSum([]int{3, 2, 4}, 6))
	fmt.Println(twoSum([]int{3, 3}, 6))
	fmt.Println(twoSum([]int{-1, -2, -3, -4, -5}, -8))

	fmt.Println("end")
}

func twoSum(nums []int, target int) []int {

	for i := 0; i < len(nums); i++ {
		current := nums[i]

		for j := 0; j < len(nums); j++ {
			if j == i {
				continue
			}

			check_element := nums[j]
			if current+check_element == target {
				result := [2]int{i, j}
				return result[:]
			}
		}
	}

	return nil
}
