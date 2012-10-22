using UnityEngine;
using System.Collections;

public class InputControls
{
    public static void AccumulateRotation(float rotation)
    {
        s_rotation += rotation;
    }

    public static float GetRotation()
    {
        return s_rotation;
    }

    public static void ClearRotation()
    {
        s_rotation = 0;
    }

    public static void AccumulateMovement(float movement)
    {
        s_movement += movement;
    }

    public static float GetMovement()
    {
        return s_movement;
    }

    public static void ClearMovement()
    {
        s_movement = 0;
    }

    public static void Jump()
    {
        s_jumping = true;
    }

    public static bool IsJumping()
    {
        return s_jumping;
    }

    public static void ClearJump()
    {
        s_jumping = false;
    }

    public static void Roll()
    {
        s_rolling = true;
    }

    public static bool IsRolling()
    {
        return s_rolling;
    }

    public static void ClearRoll()
    {
        s_rolling = false;
    }

    public static void PrimaryAttack()
    {
        s_primaryAttacking = true;
    }

    public static bool IsPrimaryAttacking()
    {
        return s_primaryAttacking;
    }

    public static void ClearPrimaryAttack()
    {
        s_primaryAttacking = false;
    }

    public static void SecondaryAttack()
    {
        s_secondaryAttacking = true;
    }

    public static bool IsSecondaryAttacking()
    {
        return s_secondaryAttacking;
    }

    public static void ClearSecondaryAttacking()
    {
        s_secondaryAttacking = false;
    }

    private static bool s_jumping;
    private static bool s_rolling;
    private static bool s_primaryAttacking;
    private static bool s_secondaryAttacking;
    private static float s_rotation;
    private static float s_movement;
}
